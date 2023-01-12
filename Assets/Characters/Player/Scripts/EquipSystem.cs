using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour
{

    [Header("Weapon and Holders")]
    public GameObject weaponHolder;
    public GameObject weapon;
    public GameObject weaponSheath;

    private GameObject currentWepHand;
    private GameObject currentWepSheath;

    void Start()
    {
        currentWepSheath = Instantiate(weapon, weaponSheath.transform);
    }

    public void DrawWeapon()
    {
        currentWepHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWepSheath);
    }

    public void SheathWeapon()
    {
        currentWepSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWepHand);
    }




}
