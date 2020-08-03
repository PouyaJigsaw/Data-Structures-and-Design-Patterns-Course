using System;
using DefaultNamespace;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 30;
    [SerializeField] private EnemyProjectile _enemyProjectilePrefab;
    private float time = 0;
    public void Initialize()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
       
       
    }


    void Update()
    {
        if (time < 5f)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
            GameObject enemyProjectile = ObjectPool.GetEnemyProjectile();
            enemyProjectile.transform.position = gameObject.transform.position;
            enemyProjectile.GetComponent<EnemyProjectile>().StartMoving();
        }

        if (transform.position.x < ScreenUtils.leftBorder)
        {
            HUD.DecrementScore();
            ObjectPool.ReturnEnemy(gameObject);
        }
        
    }

    public void StartMoving()
    {
        rb2d.AddForce(Vector2.left * speed, ForceMode2D.Force);
    }

    public void StopMoving()
    {
        rb2d.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            ObjectPool.ReturnProjectile(other.gameObject);


        }
        else if (other.CompareTag("Player"))
        {
           HUD.DecrementHealth();
        }
}
}
