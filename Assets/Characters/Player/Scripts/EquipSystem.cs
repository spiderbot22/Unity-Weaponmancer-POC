using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour
{

    [Header("Weapon and Holders")]
    public GameObject meleeWeaponHolder;
    public GameObject magicHandWeaponHolder;
    public GameObject magicSpineWeaponHolder;
    public GameObject weapon;
    public GameObject weaponSheath;

    private GameObject currentWepHand;
    private GameObject currentWepSheath;
    private Animator _animator;
    public float rotationSpeed = 1;
    private Quaternion zeroRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    private Rigidbody thrownWep;

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
                currentWepHand = Instantiate(weapon, magicHandWeaponHolder.transform);
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

    public void WepIsMagicBlocking()
    {
        _animator.SetBool("isMagicBlocking", true);
    }

    public void WepIsNotMagicBlocking()
    {
        _animator.SetBool("isMagicBlocking", false);
        currentWepHand.transform.rotation = zeroRotation; //return currentWep to original rotation
    }

    public void WepRotateWithCam(float xRotation, float yRotation)
    {
        if (_animator.GetBool("magicMode") && _animator.GetBool("isMagicBlocking") == false)
        {
            magicHandWeaponHolder.transform.rotation = Quaternion.Euler(0, xRotation, 0);
        }

        if (_animator.GetBool("magicMode") && _animator.GetBool("isMagicBlocking") == true)
        {
            magicHandWeaponHolder.transform.rotation = Quaternion.Euler(90, xRotation, -180); //rotates with camera but changes orientation of wep to face down
            currentWepHand.transform.Rotate(Vector3.up, 15.0f); //rotate wep around y-axis
        }
    }

    public void MagicBlock()
    {
        if (_animator.GetBool("isBlocking")) 
        {

        }
    }

    public void ThrowWep()
    {
        if (currentWepHand != null)
        {
            currentWepHand.transform.parent = null;
            currentWepHand.AddComponent(typeof(Rigidbody));
            thrownWep = currentWepHand.GetComponent(typeof(Rigidbody)) as Rigidbody;
            thrownWep.AddForce(Vector3.forward*3000.0f, ForceMode.Force);

            
        }
       
    }

}
