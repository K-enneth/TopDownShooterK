using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PoolingSystem : MonoBehaviour
    {
        public GameObject prefab;
        public List<GameObject> createdObjects; 
        public int poolStartSize = 10;

        private void Start()
        {
            createdObjects = new List<GameObject>();
            InitializePool();
        }
    
        private void InitializePool()
        {
            for (int indexpool = 0; indexpool < poolStartSize; indexpool++)
            {
                var obj = Instantiate(prefab, transform);
                obj.SetActive(false);
                createdObjects.Add(obj);
            }
        }

        public GameObject AskForObject(Transform posToSpawn)
        {
            for (int indexPool = 0; indexPool < createdObjects.Count; indexPool++)
            {
                if (!createdObjects[indexPool].activeInHierarchy)
                {
                    createdObjects[indexPool].transform.SetPositionAndRotation(posToSpawn.position, posToSpawn.rotation);
                    createdObjects[indexPool].SetActive(true);
                    return createdObjects[indexPool];
                }
            }
            var newObject = Instantiate(prefab,transform);
            newObject.transform.SetPositionAndRotation(posToSpawn.position, posToSpawn.rotation);
            createdObjects.Add(newObject);
            return newObject;
        
        }
    }
}
