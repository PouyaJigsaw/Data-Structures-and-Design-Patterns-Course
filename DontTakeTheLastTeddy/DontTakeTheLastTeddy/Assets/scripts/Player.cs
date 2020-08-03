﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A player
/// </summary>
public class Player : MonoBehaviour
{
    PlayerName myName;
    Timer thinkingTimer;

    // minimax search support
    Difficulty difficulty;
    int searchDepth = 0;
    MinimaxTree<Configuration> tree;

    // events invoked by class
    TurnOver turnOverEvent = new TurnOver();

    // saved for efficiency
    LinkedList<MinimaxTreeNode<Configuration>> nodeList =
        new LinkedList<MinimaxTreeNode<Configuration>>();
    List<int> binContents = new List<int>();
    List<Configuration> newConfigurations =
        new List<Configuration>();

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
	{
        // set name
		if (CompareTag("Player1"))
        {
            myName = PlayerName.Player1;
        }
        else
        {
            myName = PlayerName.Player2;
        }

        // add timer component
        thinkingTimer = gameObject.AddComponent<Timer>();
        thinkingTimer.Duration = GameConstants.AiThinkSeconds;
        thinkingTimer.AddTimerFinishedListener(HandleThinkingTimerFinished);

        // register as invoker and listener
        EventManager.AddTurnOverInvoker(this);
        EventManager.AddTakeTurnListener(HandleTakeTurnEvent);
	}
	
	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
		
	}

    /// <summary>
    /// Gets and sets the difficulty for the player
    /// </summary>
    public Difficulty Difficulty
    {
        get { return difficulty; }
        set
        {
            difficulty = value;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    searchDepth = GameConstants.EasyMinimaxDepth;
                    break;
                case Difficulty.Medium:
                    searchDepth = GameConstants.MediumMinimaxDepth;
                    break;
                case Difficulty.Hard:
                    searchDepth = GameConstants.HardMinimaxDepth;
                    break;
                default:
                    searchDepth = GameConstants.EasyMinimaxDepth;
                    break;
            }
        }
    }

    /// <summary>
    /// Adds the given listener for the TurnOver event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddTurnOverListener(
        UnityAction<PlayerName, Configuration> listener)
    {
        turnOverEvent.AddListener(listener);
    }

    /// <summary>
    /// Handles the TakeTurn event
    /// </summary>
    /// <param name="player">whose turn it is</param>
    /// <param name="boardConfiguration">current board configuration</param>
    void HandleTakeTurnEvent(PlayerName player,
        Configuration boardConfiguration)
    {
        // only take turn if it's our turn
        if (player == myName)
        {
            tree = BuildTree(boardConfiguration);
            thinkingTimer.Run();
        }
    }

    /// <summary>
    /// Builds the tree
    /// </summary>
    /// <param name="boardConfiguration">current board configuration</param>
    /// <returns>tree</returns>
    MinimaxTree<Configuration> BuildTree(
        Configuration boardConfiguration)
    {
        // build tree to appropriate depth
        MinimaxTree<Configuration> tree =
            new MinimaxTree<Configuration>(boardConfiguration);
        nodeList.Clear();
        nodeList.AddLast(tree.Root);
        while (nodeList.Count > 0)
        {
            MinimaxTreeNode<Configuration> currentNode =
                nodeList.First.Value;
            nodeList.RemoveFirst();
            List<Configuration> children =
                GetNextConfigurations(currentNode.Value);
            foreach (Configuration child in children)
            {
                // STUDENTS: only add to tree if within search depth
                if (Depth(currentNode) + 1 <= searchDepth)
                {
                    MinimaxTreeNode<Configuration> childNode =
                        new MinimaxTreeNode<Configuration>(
                            child, currentNode);
                    tree.AddNode(childNode);
                    nodeList.AddLast(childNode);
                }
            }
        }
        return tree;
    }

    /// <summary>
    /// Handles the thinking timer finishing
    /// </summary>
    void HandleThinkingTimerFinished()
    {
        // do the search and pick the move
        Minimax(tree.Root, true);

        // find child node with maximum score
        IList<MinimaxTreeNode<Configuration>> children =
            tree.Root.Children;
        MinimaxTreeNode<Configuration> maxChildNode = children[0];
        for (int i = 1; i < children.Count; i++)
        {
            if (children[i].MinimaxScore > maxChildNode.MinimaxScore)
            {
                maxChildNode = children[i];
            }
        }

        // provide new configuration as second argument
        turnOverEvent.Invoke(myName, maxChildNode.Value);
    }

    /// <summary>
    /// Gets a list of the possible next configurations
    /// given the current configuration
    /// </summary>
    /// <param name="currentConfiguration">current configuration</param>
    /// <returns>list of next configurations</returns>
    List<Configuration> GetNextConfigurations(
        Configuration currentConfiguration)
    {
        newConfigurations.Clear();
        IList<int> currentBins = currentConfiguration.Bins;
        for (int i = 0; i < currentBins.Count; i++)
        {
            int currentBinCount = currentBins[i];
            while (currentBinCount > 0)
            {
                // take one teddy from current bin
                currentBinCount--;

                // add new next configuration to list
                binContents.Clear();
                binContents.AddRange(currentBins);
                binContents[i] = currentBinCount;
                newConfigurations.Add(
                    new Configuration(binContents));
            }
        }
        return newConfigurations;
    }

    /// <summary>
    /// Assigns minimax scores to the tree nodes
    /// </summary>
    /// <param name="tree">tree to mark with scores</param>
    /// <param name="maximizing">whether or not we're maximizing</param>
    void Minimax(MinimaxTreeNode<Configuration> tree,
        bool maximizing)
    {
        // recurse on children
        IList<MinimaxTreeNode<Configuration>> children = tree.Children;
        if (children.Count > 0)
        {
            foreach (MinimaxTreeNode<Configuration> child in children)
            {
                // toggle maximizing as we move down
                Minimax(child, !maximizing);
            }

            // set default node minimax score
            if (maximizing)
            {
                tree.MinimaxScore = int.MinValue;
            }
            else
            {
                tree.MinimaxScore = int.MaxValue;
            }

            // find maximum or minimum value in children
            foreach (MinimaxTreeNode<Configuration> child in children)
            {
                if (maximizing)
                {
                    // check for higher minimax score
                    if (child.MinimaxScore > tree.MinimaxScore)
                    {
                        tree.MinimaxScore = child.MinimaxScore;
                    }
                }
                else
                {
                    // minimizing, check for lower minimax score
                    if (child.MinimaxScore < tree.MinimaxScore)
                    {
                        tree.MinimaxScore = child.MinimaxScore;
                    }
                }
            }
        }
        else
        {
            // leaf nodes are the base case
            AssignHeuristicMinimaxScore(tree, maximizing);
        }
    }

    /// <summary>
    /// Assigns the end of game minimax score
    /// </summary>
    /// <param name="node">node to mark with score</param> 
    /// <param name="maximizing">whether or not we're maximizing</param>
    void AssignEndOfGameMinimaxScore(MinimaxTreeNode<Configuration> node,
        bool maximizing)
    {
        if (maximizing)
        {
            // other player took the last teddy
            node.MinimaxScore = 1;
        }
        else
        {
            // we took the last teddy
            node.MinimaxScore = 0;
        }
    }
        
    /// <summary>
    /// Assigns a heuristic minimax score to the given node
    /// </summary>
    /// <param name="node">node to mark with score</param>
    /// <param name="maximizing">whether or not we're maximizing</param>
    void AssignHeuristicMinimaxScore(
        MinimaxTreeNode<Configuration> node,
        bool maximizing)
    {
        // might have reached an end-of-game configuration
        if (node.Value.Empty)
        {
            AssignEndOfGameMinimaxScore(node, maximizing);
        }
        else
        {
            // use a heuristic evaluation function to score the node
            /* There are two scenarios which you are sure that it reaches a specified outcome
             * In both of those there are only one teddy bear or none in bins
             * In every player's turn, they have to only pick one teddy from one bin
             * SCENARIO 1: there are 2 or 4 teddy bears. if Max picks one then min loses, and vice versa
             * SCENARIO 2: There are 1 or 3 teddy bears. Max loses cause if max picks one then min picks another and max has only one teddy to pick
             * and it will lose.
             */
            bool moreThanOneBearInOneBin = false;
            
            foreach(int bin in node.Value.Bins)
            {
                if (bin != 0 || bin != 1)
                {
                    moreThanOneBearInOneBin = true;
                    break;
                    
                }
            }

            if (!moreThanOneBearInOneBin)
            {
                //SCENARIO 1
                if (node.Value.NumBears % 2 == 0)
                {
                    if (maximizing)
                    {
                        node.MinimaxScore = 1;
                    }
                    else
                    {
                        node.MinimaxScore = 0;
                    }
                } // SCENARIO 2
                else if (node.Value.NumBears % 2 == 1)
                {
                    if (maximizing)
                    {
                        node.MinimaxScore = 0;
                    }
                    else
                    {
                        node.MinimaxScore = 1;
                    }
                }
            }
            else
            {
                node.MinimaxScore = 0.5f;
            }
        
            
           
		}
    }

    int Depth(MinimaxTreeNode<Configuration> node)
    {
      
        MinimaxTreeNode<Configuration> parentNode = node.Parent;

        if (parentNode == null)
        {
            //it is root
            return 0;
        }
        
        int depth = 1;
        
        while (parentNode != null)
        {
            parentNode = parentNode.Parent;
            depth++;
        }

        return depth;
    }
}
