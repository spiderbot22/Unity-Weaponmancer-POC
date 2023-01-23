using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour
{

    [Header("Weapon and Holders")]
    public GameObject meleeWeaponHolder;
    public GameObject magicWeaponHolder;
    public GameObject weapon;
    public GameObject weaponSheath;

    private GameObject currentWepHand;
    private GameObject currentWepSheath;
    private Animator _animator;

    void Start()
    {
        currentWepSheath = Instantiate(weapon, weaponSheath.transform);
        _animator = GetComponent<Animator>();
    }

    public void DrawWeapon()
    {
        if (currentWepHand == null && currentWepSheath != null)
        {
            if (_animator.GetBool("meleeMode")) 
            {
                currentWepHand = Instantiate(weapon, meleeWeaponHolder.transform);
                Destroy(currentWepSheath);
            }

            if (_animator.GetBool("magicMode"))
            {
                Debug.Log("fire");
                currentWepHand = Instantiate(weapon, magicWeaponHolder.transform);
                Destroy(currentWepSheath);
            }
        } 
    }

    public void SheathWeapon()
    {
        if (currentWepHand != null && currentWepSheath == null)
        {
            currentWepSheath = Instantiate(weapon, weaponSheath.transform);
            Destroy(currentWepHand);
        }
    }

    public void WepIsDrawn()
    {
        _animator.SetBool("inCombat", true);
    }

    public void WepIsSheathed()
    {
        _animator.SetBool("inCombat", false);
    }

}
