using System;
using System.Collections.Generic;
using System.Security;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        public PoolingSystem enemyPool;
        public int currentWave;
        [SerializeField] private int amount;

        [SerializeField]private List<Transform> _spawnPoints;
        private GameObject _spawnHolder;
        private float _newSpeed =1.5f;

        [SerializeField]private List<GameObject> currentEnemies;
        
        public delegate void OnWaveFinished(int waveNumber);
        public static event OnWaveFinished WaveFinished;


        private void OnEnable()
        {
            Enemy.DeadEvent += CheckEnemies;
        }

        private void OnDisable()
        {
            Enemy.DeadEvent -= CheckEnemies;
        }

        private void Start()
        {
            _spawnHolder = GameObject.Find("Spawn Positions");
            for (var indexSpawn = 0; indexSpawn < _spawnHolder.transform.childCount; indexSpawn++)
            {
                _spawnPoints.Add(_spawnHolder.transform.GetChild(indexSpawn));
            }
            
            SpawnEnemies(amount);
        }

        public void NewWave()
        {
            amount += 3;
            SpawnEnemies(amount);
        }
        
        private void SpawnEnemies(int newAmount)
        {
            var spawnedEnemies = 0;
            _newSpeed+= 0.5f;
            while (newAmount > spawnedEnemies)
            {
               var newEnemy= enemyPool.AskForObject(_spawnPoints[Random.Range(0, _spawnPoints.Count)]);
               currentEnemies.Add(newEnemy);
               newEnemy.GetComponent<Enemy>().speed = _newSpeed;
               spawnedEnemies++;
            }
        }

        private void CheckEnemies()
        {
            for (int indexEnemy = 0; indexEnemy < currentEnemies.Count; indexEnemy++)
            {
                if (!currentEnemies[indexEnemy].gameObject.activeInHierarchy)
                {
                    currentEnemies.RemoveAt(indexEnemy);
                    
                    if (currentEnemies.Count == 0)
                    {
                        currentWave++;
                        WaveFinished?.Invoke(currentWave);
                    }
                }
            }
        }
        
    }
}
