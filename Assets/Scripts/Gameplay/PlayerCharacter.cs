using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private void Start()
    {
        UIManager.Instance.InitHealthBar(HP);
    }
}
