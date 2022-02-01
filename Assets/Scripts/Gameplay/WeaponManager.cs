using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public Transform weaponAnchor;
    private PlayerBehaviour playerBehaviour;
    private List<GameObject> weapons = new List<GameObject>();
    private GameObject currentWeapon;
    private Gun currentGun;
    private Transform currentBulletBownTrans;

    private void Awake()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    void Start ()
    {
        for (int i = 0; i < weaponAnchor.childCount; i++)
        {
            AddWeapon(weaponAnchor.GetChild(i).gameObject);
        }
        SelectWeaponByIndex(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            GameObject weapon = other.gameObject;
            
            AddWeapon(weapon);
            SelectWeaponByIndex(weapons.Count - 1);
        }
    }

    // ===========================================
    // public function ===========================
    // ===========================================
    public void AddWeapon(GameObject weapon)
    {
        if (weapon != null && weapons.Exists( x => x.name == weapon.name) == false)
        {
            weapon.transform.parent = weaponAnchor;

            weapon.GetComponent<Collider>().enabled = false;
            weapon.GetComponent<AutoRotate>().enabled = false;

            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
            weapon.transform.localScale = Vector3.one;
            weapons.Add(weapon);
        }
    }

    public Gun GetCurrentGun()
    {
        if (currentGun != null)
            return currentGun;
        else
            return null;
    }

    public Transform GetCurrentBulletBownTrans()
    {
        return currentBulletBownTrans;
    }

    // ===========================================
    // private function ==========================
    // ===========================================
    private void SelectWeaponByIndex(int index)
    {
        if (currentWeapon != null)
            currentWeapon.SetActive(false);

        currentWeapon = index < weapons.Count ? weapons[index] : null;

        currentGun = currentWeapon.GetComponent<Gun>();
        currentBulletBownTrans = currentWeapon.transform.GetChild(0);

        if (currentWeapon != null)
        {
           currentWeapon.SetActive(true);
        }

        playerBehaviour.SetShootingSpeed(currentGun.ShootingSpeed());

        Debug.Log(currentWeapon.name);
    }

}
