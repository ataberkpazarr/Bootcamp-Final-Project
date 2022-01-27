using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private GameObject poolObject;
    [SerializeField] private int poolSize;
    private List<GameObject> pool;

    private void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            var objectForPool = Instantiate(poolObject);
            objectForPool.gameObject.SetActive(false);
            pool.Add(objectForPool);
        }
    }

    public GameObject GetObject(Vector3 position)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
            {
                pool[i].transform.position = position;
                pool[i].transform.rotation = Quaternion.identity;
                return pool[i];
            }
        }

        return null;
    }
}
