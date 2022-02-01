using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Singleton<BulletSpawner> {

    public class BulletInfo
    {
        public GameObject bullet;
        public GameObject owner;
    }

    [Range(1f, 100f)]
    public float speed = 10f;
    public float life = 3f;

    public BulletInfo Spawn(Vector3 position, Quaternion rotation, GameObject owner)
    {
        GameObject bulletObj = ObjectPooler.Instance.SpawnFormPool(ObjectPooler.ObjectTag.Rifle_Bullet, position, rotation);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SetSpawnPosition(position);

        BulletInfo bulletInfo = new BulletInfo();
        bulletInfo.bullet = bulletObj;
        bulletInfo.owner = owner;

        bullet.Fire(speed, life);

        return bulletInfo;
    }
}
