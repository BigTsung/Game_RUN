using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootingSpeed = 0.25f;

    private Transform mTransform;

    private void Start()
    {
        mTransform = this.transform;
        InvokeRepeating("Fire", 0, shootingSpeed);
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, mTransform.position, mTransform.rotation);
        //Debug.Log(mTransformm.forward);
        bullet.GetComponent<SimpleBullet>().Init(mTransform.forward);
    }

    //void Update()
    //{

    //}
}
