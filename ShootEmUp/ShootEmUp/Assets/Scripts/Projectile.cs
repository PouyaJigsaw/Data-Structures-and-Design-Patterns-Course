using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;


public class Projectile : MonoBehaviour
{
    private static int score = 0;
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 200;
    private EnemySpawner _enemySpawner;
    // Start is called before the first frame update
     public void Initialize()
    { 
        
        rb2d = GetComponent<Rigidbody2D>();
       
    }

     private void Awake()
     {
         _enemySpawner = Camera.main.GetComponent<EnemySpawner>();
        
     }

     private void Update()
     {
         if (transform.position.x > ScreenUtils.rightBorder)
         {
             ObjectPool.ReturnProjectile(gameObject);
         }
     }

     public void StartMoving()
    {
       
            rb2d.AddForce(Vector2.right * speed, ForceMode2D.Force);
        
    }

    public void StopMoving()
    {
        rb2d.velocity = Vector2.zero;
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
          ObjectPool.ReturnEnemy(other.gameObject);
          _enemySpawner.SpawnEnemy();
          score += 10;
          HUD.IncrementScore();

        }
    }
   
}
