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
    public Transform player;

    private GameObject currentWepHand;
    private GameObject currentWepSheath;
    private Animator _animator;
    public float magicBlockWepRotationSpeed = 30.0f;
    private Quaternion zeroRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    private Rigidbody thrownWepRB;
    Ray ray;

    void Start()
    {
        currentWepSheath = Instantiate(weapon, weaponSheath.transform);
        _animator = GetComponent<Animator>();
        
    }

    public void Update()
    {
        ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
        Debug.Log(ray.direction);
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
           magicHandWeaponHolder.transform.rotation = Quaternion.Euler(-yRotation, xRotation, 0);
        }

        //rotate weapon horizontally when blocking
        if (_animator.GetBool("magicMode") && _animator.GetBool("isMagicBlocking") == true)
        {
            magicHandWeaponHolder.transform.rotation = Quaternion.Euler(90, xRotation, -180); //rotates with camera but changes orientation of wep to face down
            currentWepHand.transform.Rotate(Vector3.up, magicBlockWepRotationSpeed); //rotate wep around y-axis
        }
    }

    public void ThrowWep()
    {
        if (currentWepHand.GetComponent(typeof(Rigidbody)) == null)
        {
            Debug.DrawRay(magicHandWeaponHolder.transform.position, ray.direction * 100, Color.red, 2f);

            currentWepHand.transform.parent = null;
            currentWepHand.AddComponent(typeof(Rigidbody));
            thrownWepRB = currentWepHand.GetComponent(typeof(Rigidbody)) as Rigidbody;
            thrownWepRB.AddForce(ray.direction * 3000.0f, ForceMode.Force);

        }
       
    }

}
