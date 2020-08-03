
using System.Collections.Generic;
using UnityEngine;

    public static class ObjectPool
    {
        
        public static void Initialize()
        { 
            projectilePrefab = Resources.Load<GameObject>("Projectile");
            projectilePool = new List<GameObject>(20);

            for (int i = 0; i < 20; i++)
            {
                projectilePool.Add(CreateNewProjectile());
            }
            
            enemyPrefab = Resources.Load<GameObject>("Enemy");
            enemyPool = new List<GameObject>(20);


            for (int i = 0; i < 20; i++)
            {
                enemyPool.Add(CreateNewEnemy());
            }

            enemyProjectilePrefab = Resources.Load<GameObject>("EnemyProjectile");
            enemyProjectilePool = new List<GameObject>(30);

            for (int i = 0; i < 30; i++)
            {
                enemyProjectilePool.Add(CreateNewEnemyProjectile());
            }
        }
        
        
        #region Projectile
        
       
        private static GameObject projectilePrefab;
        private static List<GameObject> projectilePool;


         static  int projectileCount
        {
            get { return projectilePool.Count; }
        }

         public static void ReturnProjectile(GameObject projectile)
        {
            projectilePool.Add(projectile);
            projectile.SetActive(false);
            projectile.GetComponent<Projectile>().StopMoving();
        }

        public static GameObject GetProjectile()
        {
            if (projectilePool.Count > 0)
            {
                GameObject lastProjectile = projectilePool[projectileCount - 1];
                projectilePool.RemoveAt(projectileCount - 1);
                lastProjectile.SetActive(true);
                return lastProjectile;
            }
            else
            {
                projectilePool.Capacity++;
                return CreateNewProjectile();
            }
            
          
        }

        public static GameObject CreateNewProjectile()
        {
            GameObject newProjectile = GameObject.Instantiate(projectilePrefab);
            newProjectile.GetComponent<Projectile>().Initialize();
            newProjectile.SetActive(false);
            return newProjectile;
        }
        #endregion

        #region Enemy

        private static GameObject enemyPrefab;
        private static List<GameObject> enemyPool;

        static int enemyCount
        {
            get { return enemyPool.Count; }
        }
        
        public static void ReturnEnemy(GameObject enemy)
        {
            enemyPool.Add(enemy);
            enemy.SetActive(false);
            enemy.GetComponent<Enemy>().StopMoving();
        }

        public static GameObject GetEnemy()
        {
            if (enemyPool.Count > 0)
            {
                GameObject lastEnemy = enemyPool[enemyCount - 1];
                enemyPool.RemoveAt(enemyCount - 1);
                lastEnemy.SetActive(true);
                return lastEnemy;
            }
            else
            {
                enemyPool.Capacity++;
                return CreateNewEnemy();
            }
        }

        public static GameObject CreateNewEnemy()
        {
            GameObject newEnemy = GameObject.Instantiate(enemyPrefab);
            newEnemy.GetComponent<Enemy>().Initialize();
            newEnemy.SetActive(false);
            return newEnemy;
        }
        #endregion
        
        #region EnemyProjectile

        private static GameObject enemyProjectilePrefab;
        private static List<GameObject> enemyProjectilePool;


        public static int enemyProjectileCount
        {
            get { return enemyProjectilePool.Count; }
        }
        public static GameObject CreateNewEnemyProjectile()
        {
            GameObject newEnemyProjectile = GameObject.Instantiate(enemyProjectilePrefab);
            newEnemyProjectile.GetComponent<EnemyProjectile>().Initialize();
            newEnemyProjectile.SetActive(false);
            return newEnemyProjectile;
        }

        public static void ReturnEnemyProjectile(GameObject enemyProjectile)
        {
            enemyProjectilePool.Add(enemyProjectile);
            enemyProjectile.SetActive(false);
            enemyProjectile.GetComponent<EnemyProjectile>().StopMoving();
        }

        public static GameObject GetEnemyProjectile()
        {
            if (enemyProjectilePool.Count > 0)
            {
                GameObject lastEnemyProjectile = enemyProjectilePool[enemyProjectileCount - 1];
                enemyProjectilePool.RemoveAt(enemyProjectileCount - 1);
                lastEnemyProjectile.SetActive(true);
                return lastEnemyProjectile;
            }
            else
            {
                enemyProjectilePool.Capacity++;
                return CreateNewEnemyProjectile();
            }
        }
        #endregion
    }
