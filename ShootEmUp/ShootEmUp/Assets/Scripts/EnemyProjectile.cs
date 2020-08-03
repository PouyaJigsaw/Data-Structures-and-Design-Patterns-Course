using System;
using UnityEngine;


    public class EnemyProjectile : MonoBehaviour
    {
        private Rigidbody2D rb2d;
        public int speed;

        public void Initialize()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        public void StartMoving()
        {
            rb2d.AddForce(Vector2.left * speed, ForceMode2D.Force);
            
        }

        public void StopMoving()
        {
            rb2d.velocity = Vector2.zero;
        }
        private void Update()
        {
            if (transform.position.x < ScreenUtils.leftBorder)
            {
                ObjectPool.ReturnEnemyProjectile(gameObject);
               
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
              HUD.DecrementHealth();
            }
        }
    }
