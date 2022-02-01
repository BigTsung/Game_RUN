using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler> {

    [System.Serializable]
    public class Pool
    {
        public ObjectTag tag;
        public GameObject prefab;
        public int size;
    }

    public enum ObjectTag
    {
        ZombieD,
        Rifle_Bullet
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<ObjectTag, Queue<GameObject>> poolDictionary = new Dictionary<ObjectTag, Queue<GameObject>>();

    // Use this for initialization
    void Start()
    {
        foreach(Pool pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.prefab.name + "_" + i;
                obj.transform.parent = this.transform;
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objPool);
        }
    }

    public GameObject SpawnFormPool(ObjectTag tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist!");
            return null;
        }
        GameObject obj = poolDictionary[tag].Dequeue();

        obj.SetActive(true);
        obj.transform.GetChild(0).position = Vector3.zero;
        obj.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.up * 180f);

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(obj);

        return obj;
    }
}
