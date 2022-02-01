using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUIManager : MonoBehaviour {

	public Slider healthBar;
    public Text healthBarVal;

    public Slider overheadBar;

    private int maxHP;

    public void InitHealthBar(int value)
    {
        if (healthBar != null && healthBarVal != null)
        {
            healthBar.maxValue = value;
            healthBar.value = value;
            healthBarVal.text = value.ToString() + "/" + value.ToString();

            maxHP = value;
        } 
    }

    //public void InitOverheadBar(int value)
    //{
    //    if (overheadBar != null)
    //    {
    //        healthBar.maxValue = value;
    //        healthBar.value = value;
    //        healthBarVal.text = value.ToString() + "/" + value.ToString();

    //        maxHP = value;
    //    }
    //}

    public void RefreshHealthBar(int value)
    {
        if (healthBar != null && healthBarVal != null)
        {
            //Debug.Log(value + " " + maxHP);
        
            healthBar.value = value;
            healthBarVal.text = value + "/" + maxHP;
        }
    }

    public void RefreshOverheadBar(float value)
    {
        if (overheadBar != null)
        {
            overheadBar.value = value;
        }
    }
}
