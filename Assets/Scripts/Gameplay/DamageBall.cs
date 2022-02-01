using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageBall : MonoBehaviour {

    public LayerMask ignoreLayers;

    public bool isDisableOnCollision = true;
    public bool isDestroyOnCollision = false;

    public int damageValue = 10;

    private Collider colliderCom;

    public delegate void OnCollision(Collider collision);
    public OnCollision onCollisionEnter;

    // Use this for initialization
    void Start () {
        colliderCom = GetComponent<Collider>();

        if (colliderCom != null)
            colliderCom.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool inLayerMask = ignoreLayers == (ignoreLayers | (1 << other.gameObject.layer));

        if (inLayerMask == false)
            return;

        if (onCollisionEnter != null)
            onCollisionEnter(other);

        if (isDisableOnCollision)
            gameObject.SetActive(false);

        if (isDestroyOnCollision)
            Destroy(gameObject);

        Character character = other.GetComponent<Character>();

        if(character != null && !character.Dead)
        {
            character.HP -= damageValue;
        }
    }
}
