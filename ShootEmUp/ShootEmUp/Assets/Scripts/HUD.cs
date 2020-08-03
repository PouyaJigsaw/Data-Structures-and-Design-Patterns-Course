using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
  [SerializeField] Text textHealth;
  [SerializeField] Text textScore;

  private static int score;

  private static int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.text = health.ToString();
        textScore.text = score.ToString();
    }

    public static void DecrementHealth()
    {
        health -= 10;
    }

    public static void IncrementScore()
    {
        score += 10;
    }

    public static void DecrementScore()
    {
        score -= 10;
    }
}
