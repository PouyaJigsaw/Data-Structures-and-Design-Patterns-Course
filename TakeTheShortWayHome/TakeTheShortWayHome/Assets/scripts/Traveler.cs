using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A traveler
/// </summary>
public class Traveler : MonoBehaviour
{
    // events fired by class
    PathFoundEvent pathFoundEvent = new PathFoundEvent();
    PathTraversalCompleteEvent pathTraversalCompleteEvent = new PathTraversalCompleteEvent();
    private LinkedList<SearchNode<Waypoint>> pathSearchNodes;

    [SerializeField] private float forcePulseMagnitude;

    private Rigidbody2D rb2d;

    private void Awake()
    {
	    rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
	/// Use this for initialization
	/// </summary>
	void Start()
	{
		pathSearchNodes = Search();
		MoveToTarget();
		
		
		
	}

	void MoveToTarget()
	{
		
		LinkedListNode<SearchNode<Waypoint>> target = pathSearchNodes.First;
		if (target == null)
		{
			pathTraversalCompleteEvent.Invoke();
			return;
		}
		pathSearchNodes.RemoveFirst();
		Vector2 targetPosition = target.Value.GraphNode.Value.transform.position;
		
		Vector2 direction = new Vector2(
				targetPosition.x - transform.position.x,
				targetPosition.y - transform.position.y
				);
		direction.Normalize();
		rb2d.AddForce(direction * forcePulseMagnitude);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		
		rb2d.velocity = Vector2.zero;
		
		if (other.tag != "Start" && other.tag != "End")
		{
			other.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
		}
		MoveToTarget();
		
	}

	/// <summary>
	/// Update is called once per frame
	/// </summary>
	void Update()
	{
		
	}

	LinkedList<SearchNode<Waypoint>> Search()
	{
		SortedLinkedList<SearchNode<Waypoint>> searchList = new SortedLinkedList<SearchNode<Waypoint>>();
		Dictionary<SearchNode<Waypoint>, GraphNode<Waypoint>> searchDictionary = new Dictionary<SearchNode<Waypoint>, GraphNode<Waypoint>>();
		LinkedList<SearchNode<Waypoint>> pathNodes = new LinkedList<SearchNode<Waypoint>>();
		
		Waypoint start = GameObject.FindGameObjectWithTag("Start").GetComponent<Waypoint>();
		Waypoint end = GameObject.FindGameObjectWithTag("End").GetComponent<Waypoint>();

		for (int i = 0; i < GraphBuilder.Graph.Count; i++)
		{
			GraphNode<Waypoint> node = GraphBuilder.Graph.Nodes[i];
			SearchNode<Waypoint> searchNode = new SearchNode<Waypoint>(node);

			
			if (node.Value.Equals(start))
			{
				searchNode.Distance = 0;
			}
			
			searchList.Add(searchNode);
			searchDictionary.Add(searchNode,node);
			
		}

		while (searchList.Count != 0)
		{
			SearchNode<Waypoint> currentSearchNode = searchList.First.Value;
			searchList.Remove(currentSearchNode);

			GraphNode<Waypoint> currentGraphNode = searchDictionary[currentSearchNode];
			searchDictionary.Remove(currentSearchNode);

			if (currentGraphNode.Value.Equals(end))
			{
				pathFoundEvent.Invoke(currentSearchNode.Distance);
				
				pathNodes.AddFirst(currentSearchNode);
				SearchNode<Waypoint> previous = currentSearchNode.Previous;

				while (previous != null)
				{
					pathNodes.AddFirst(previous);
					previous = previous.Previous;
				}

				return pathNodes;


			}

			foreach (var neighbor in currentGraphNode.Neighbors)
			{
				if (searchDictionary.ContainsValue(neighbor))
				{
					float newDistance = currentSearchNode.Distance + currentGraphNode.GetEdgeWeight(neighbor);
					
					SearchNode<Waypoint> neighborSearchNode = searchDictionary.FirstOrDefault(x => x.Value.Equals(neighbor)).Key;
					

					if (newDistance < neighborSearchNode.Distance)
					{
						neighborSearchNode.Distance = newDistance;
						neighborSearchNode.Previous = currentSearchNode;
						searchList.Reposition(neighborSearchNode);
					}
					
					
				}
			}

			

		}

		Debug.Log("WI");
		return pathNodes;
	}

	string pathNodesString()
	{
		StringBuilder pathString = new StringBuilder();
		SearchNode<Waypoint> searchNode = pathSearchNodes.First.Value;

		
		int nodeCount = 0;
		
		while (searchNode != null)
		{
			nodeCount++;
			pathString.Append("[Node Id: " + searchNode.GraphNode.Value.Id + " Edge Weight: " +
			                  searchNode.GraphNode.GetEdgeWeight(searchNode.Previous.GraphNode) + "]");
			if (nodeCount < pathSearchNodes.Count)
			{
				pathString.Append(" ");
			}

			searchNode = searchNode.Previous;
		}

		return pathString.ToString();

	}
    /// <summary>
    /// Adds the given listener for the PathFoundEvent
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddPathFoundListener(UnityAction<float> listener)
    {
        pathFoundEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the PathTraversalCompleteEvent
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddPathTraversalCompleteListener(UnityAction listener)
    {
        pathTraversalCompleteEvent.AddListener(listener);
    }
}
