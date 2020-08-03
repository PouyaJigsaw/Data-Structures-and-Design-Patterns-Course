using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Game manager
/// </summary>
public class DontTakeTheLastTeddy : MonoBehaviour
{
    Board board;
    Player player1;
    Player player2;

    [SerializeField] private int numberOfGames = 5;
    [SerializeField] private Difficulty player1_Difficulty;
    [SerializeField] private Difficulty player2_Difficulty;
    private PlayerName playerTurn = PlayerName.Player1;
    private int gamesDone = 0;
    Difficulty[,] _difficulties = new Difficulty[,]
    {
        {Difficulty.Easy,Difficulty.Easy},
        {Difficulty.Medium,Difficulty.Medium},
        {Difficulty.Hard,Difficulty.Hard},
        {Difficulty.Easy,Difficulty.Medium},
        {Difficulty.Easy,Difficulty.Hard},
        {Difficulty.Medium,Difficulty.Hard}
        
    };

    private int diffChangeCount = 0;
    
        // events invoked by class
    TakeTurn takeTurnEvent = new TakeTurn();
    GameOver gameOverEvent = new GameOver();
    GameStarting gameStarting = new GameStarting();

    private Timer pauseBetweenGamesTimer;

    /// <summary>
    /// Awake is called before Start
    /// </summary>
    void Awake()
    {
        // retrieve board and player references
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();

        // register as invoker and listener
        EventManager.AddTakeTurnInvoker(this);
        EventManager.AddGameOverInvoker(this);
        EventManager.AddGameStartingInvoker(this);
        EventManager.AddTurnOverListener(HandleTurnOverEvent);
        EventManager.AddGameStartingListener(HandleGameStartingEvent);


        pauseBetweenGamesTimer = gameObject.AddComponent<Timer>();
        pauseBetweenGamesTimer.Duration = GameConstants.pauseBetweenGames;
        pauseBetweenGamesTimer.AddTimerFinishedListener(HandlePauseTimerFinishedEvent);
    }

	/// <summary>
	/// Use this for initialization
	/// </summary>
	void Start()
	{
        StartGame(playerTurn, player1_Difficulty, player2_Difficulty);
    }

    /// <summary>
    /// Adds the given listener for the TakeTurn event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddTakeTurnListener(UnityAction<PlayerName, Configuration> listener)
    {
        takeTurnEvent.AddListener(listener);
    }

    /// <summary>
    /// Adds the given listener for the GameOver event
    /// </summary>
    /// <param name="listener">listener</param>
    public void AddGameOverListener(UnityAction<PlayerName, Difficulty,Difficulty> listener)
    {
        gameOverEvent.AddListener(listener);
    }

    public void AddGameStartingListener(UnityAction listener)
    {
        gameStarting.AddListener(listener);
    }
    /// <summary>
    /// Starts a game with the given player taking the
    /// first turn
    /// </summary>
    /// <param name="firstPlayer">player taking first turn</param>
    /// <param name="player1Difficulty">difficulty for player 1</param>
    /// <param name="player2Difficulty">difficulty for player 2</param>
    void StartGame(PlayerName firstPlayer, Difficulty player1Difficulty,
        Difficulty player2Difficulty)
    {
        
        // set player difficulties
        player1.Difficulty = player1Difficulty;
        player2.Difficulty = player2Difficulty;

        // create new board
        board.CreateNewBoard();
        takeTurnEvent.Invoke(firstPlayer,
            board.Configuration);
    }

    /// <summary>
    /// Handles the TurnOver event by having the 
    /// other player take their turn
    /// </summary>
    /// <param name="player">who finished their turn</param>
    /// <param name="newConfiguration">the new board configuration</param>
    void HandleTurnOverEvent(PlayerName player, 
        Configuration newConfiguration)
    {
        board.Configuration = newConfiguration;

        // check for game over
        if (newConfiguration.Empty)
        {
            // fire event with winner
                if (player == PlayerName.Player1)
                {
                    gameOverEvent.Invoke(PlayerName.Player2, player1_Difficulty, player2_Difficulty);
                    pauseBetweenGamesTimer.Run();
                   
                    
                }
                else
                {
                    gameOverEvent.Invoke(PlayerName.Player1, player1_Difficulty, player2_Difficulty);
                    pauseBetweenGamesTimer.Run();
              
                }
        }
        else
        {
            // game not over, so give other player a turn
            if (player == PlayerName.Player1)
            {
                takeTurnEvent.Invoke(PlayerName.Player2,
                    newConfiguration);
            }
            else
            {
                takeTurnEvent.Invoke(PlayerName.Player1,
                    newConfiguration);
            }
        }
    }


    void HandlePauseTimerFinishedEvent()
    {
        gamesDone++;
        
        if(gamesDone < numberOfGames)
            gameStarting.Invoke();
        else
        {
            SceneManager.LoadScene("scenes/statistics");
        }
    }
    
    void HandleGameStartingEvent()
    {
        
        PlayerName nextPlayer;
        
        if (playerTurn == PlayerName.Player1)
        {
            nextPlayer = PlayerName.Player2;
        }
        else
        {
            nextPlayer = PlayerName.Player1;
        }

        playerTurn = nextPlayer;
        
        ChangeDifficulty();
        StartGame(nextPlayer, player1_Difficulty, player2_Difficulty);
        
    }

    void ChangeDifficulty()
    {
        if (gamesDone % 100 == 0)
        {
            diffChangeCount++;
            player1_Difficulty = _difficulties[diffChangeCount, 0];
            player2_Difficulty = _difficulties[diffChangeCount, 1];
        }
    }
    
    
}
