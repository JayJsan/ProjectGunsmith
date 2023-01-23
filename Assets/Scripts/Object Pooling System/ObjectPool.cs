using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /*
    https://www.youtube.com/watch?v=YCHJwnmUGDk
    Code taken and modified from Introduction to Object Pooling in Unity - bendux;
    Added a holder and expanding ability.



    */

    private List<GameObject> pooledObjects = new List<GameObject>();
    public int amountToPool = 20;
    public int maxPoolableObjects = 30;
    public int pooledObjectsCount = 0;
    public bool canExpand = true;
    public bool hasHolder = true;
    [SerializeField] private GameObject objectPoolHolder;
    [SerializeField] private GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj;
            if (hasHolder)
            {
                obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, objectPoolHolder.transform);
            }
            else
            {
                obj = Instantiate(prefab);
            }

            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        pooledObjectsCount = pooledObjects.Count;
    }

    public GameObject GetPooledObject()
    {
        pooledObjectsCount = pooledObjects.Count;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        // If it gets to this point then there are no more objects in which case we expand
        return ExpandPool();
    }

    public GameObject ExpandPool()
    {
        if (!(pooledObjects.Count == maxPoolableObjects))
        {
            GameObject obj;
            if (hasHolder)
            {
                obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, objectPoolHolder.transform);
            }
            else
            {
                obj = Instantiate(prefab);
            }

            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
        pooledObjectsCount = pooledObjects.Count;
        return null;
    }
}
