using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private GameObject meleeEnemyPrfab;
    [SerializeField] private float spawnSpeed = 1f;

    private void Start()
    {
        InvokeRepeating("Spawn", 0, spawnSpeed);    
    }

    private void Spawn()
    {
        Instantiate(meleeEnemyPrfab, this.transform);
    }
}
