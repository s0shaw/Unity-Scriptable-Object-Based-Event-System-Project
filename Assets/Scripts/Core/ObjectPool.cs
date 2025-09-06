using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private Transform poolParent;

    private Queue<GameObject> pool;

    private void Awake()
    {
        if (poolParent == null)
        {
            poolParent = this.transform;
        }

        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectToPool, poolParent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.SetParent(null);
            obj.SetActive(true);
            return obj;
        }
        
        return null;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        pool.Enqueue(obj);
    }
} 