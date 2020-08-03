using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
   public static int[,] results = new int[6,2];
    
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        EventManager.AddGameOverListener(StoreData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void StoreData(PlayerName winningPlayer, Difficulty firstPlayerDifficulty, Difficulty secondPlayerDifficulty)
    {
        int row = 0;
        int column = 0;
        
        if (winningPlayer == PlayerName.Player1)
        {
            column = 0;
        }
        else
        {
            column = 1;
        }

        if (firstPlayerDifficulty == Difficulty.Easy)
        {
            if (secondPlayerDifficulty == Difficulty.Easy)
            {
                row = 0;
            }
            else if (secondPlayerDifficulty == Difficulty.Medium)
            {
                row = 3;
            }
            else
            {
                row = 4;
            }
        }
        else if (firstPlayerDifficulty == Difficulty.Medium)
        {
            if (secondPlayerDifficulty == Difficulty.Easy)
            {
                throw new ArgumentException("Wrong!");
            }
            if (secondPlayerDifficulty == Difficulty.Medium)
            {
                row = 1;
            }
            else if (secondPlayerDifficulty == Difficulty.Hard)
            {
                row = 5;
            }
        }
        else if (firstPlayerDifficulty == Difficulty.Hard)
        {
            if (secondPlayerDifficulty == Difficulty.Easy)
            {
                throw new ArgumentException("Wrong!");
            }
            if (secondPlayerDifficulty == Difficulty.Medium)
            {
                throw new ArgumentException("Wrong!");
            }
            else if (secondPlayerDifficulty == Difficulty.Hard)
            {
                row = 2;
            }
        }

        results[row, column]++;
    }
}
