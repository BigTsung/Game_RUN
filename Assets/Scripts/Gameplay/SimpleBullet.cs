using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBullet : MonoBehaviour
{
    [SerializeField] private float life = 5f;
    [SerializeField] private float speed = 10f;

    private Rigidbody bulletRigidbody;


    //private void Awake()
    //{

    //}

    private void Start()
    {
        Invoke("ReadyToDie", life);
    }

    private void ReadyToDie()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

    public void Init(Vector3 direction)
    {
        if(bulletRigidbody == null)
            bulletRigidbody = GetComponent<Rigidbody>();

        bulletRigidbody.AddForce(direction * speed);
    }
}
