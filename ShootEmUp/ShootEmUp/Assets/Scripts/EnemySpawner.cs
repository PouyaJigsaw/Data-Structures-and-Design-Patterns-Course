using System;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
    {
        private float time = 0;
        [SerializeField] private GameObject enemyPrefab;
        private float enemyColliderExtent;
        
        //saved for efficiency
        private float bottom;
        private float top;
        private float randomCoordinateY;
        private float coordinateX;
        

        private void Start()
        {
            GameObject enemyTemp = Instantiate(enemyPrefab);
            enemyColliderExtent = enemyTemp.GetComponent<BoxCollider2D>().bounds.extents.x;
            Destroy(enemyTemp);
            
            
            bottom = ScreenUtils.bottomBorder + enemyColliderExtent;
            top = ScreenUtils.topBorder - enemyColliderExtent;
            coordinateX = ScreenUtils.rightBorder;
            
            SpawnEnemy();         
            
        }

        private void Update()
        {
            if (time < 4)
            {
                time += Time.deltaTime;
                
            }
            else
            {
                time = 0;
                SpawnEnemy();
            }
        }

        public void SpawnEnemy()
        {
            GameObject enemy = ObjectPool.GetEnemy();
            enemy.transform.position = RandomSpawnPosition();
            enemy.GetComponent<Enemy>().StartMoving();
        }

        Vector2 RandomSpawnPosition()
        {
            randomCoordinateY = Random.Range(bottom, top);
            return new Vector2(coordinateX, randomCoordinateY);
        }
    }
