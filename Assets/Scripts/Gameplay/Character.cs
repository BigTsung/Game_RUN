using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp = 0;
    [SerializeField]
    private bool isDead = false;

    public delegate void OnDead();
    public OnDead onDead;

    public delegate void OnDamage(int hurtVal);
    public OnDamage onDamage;

    private int oriHp = 0;
    private bool oriDead;

    private void Awake()
    {
        oriHp = hp;
        oriDead = isDead;
    }

    private void OnEnable()
    {
        hp = oriHp;
        isDead = oriDead;
    }

    public bool Dead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public int HP
    {
        get { return hp; }
        set 
        {
            hp = value;
            
            if (hp <= 0)
            {
                hp = 0;

                if (onDead != null && !isDead)
                {
                    isDead = true;
                    onDead();
                }
            }

            if (onDamage != null && !isDead)
                onDamage(hp);
        }
    }
}
