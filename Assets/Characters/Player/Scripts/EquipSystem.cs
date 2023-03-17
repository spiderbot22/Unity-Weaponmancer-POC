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

    [Header("Weapon Throwing and Blocking")]
    public LayerMask layersToHit;
    public float throwForce;
    public float magicBlockWepRotationSpeed = 30.0f;

    private float maxDistance = 100;
    private GameObject currentWepHand;
    private GameObject currentWepSheath;
    private Animator _animator;
    private Quaternion zeroRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
    private Rigidbody thrownWepRB;

    void Start()
    {
        currentWepSheath = Instantiate(weapon, weaponSheath.transform);
        _animator = GetComponent<Animator>();
        
    }

    public void Update()
    {
        
    }
    
    public void SwapHolder()
    {
        if (_animator.GetBool("meleeMode"))
        {
            Destroy(currentWepHand);
            currentWepHand = Instantiate(weapon, meleeWeaponHolder.transform);
        }

        if (_animator.GetBool("magicMode"))
        {
            Destroy(currentWepHand);
            currentWepHand = Instantiate(weapon, magicHandWeaponHolder.transform);
        }

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

    public void WepRotateWithCam(float xRotation, float yRotation) //called from PlayerController
    {
        if (_animator.GetBool("magicMode") && _animator.GetBool("isMagicBlocking") == false)
        {
            //magicHandWeaponHolder.transform.rotation = Quaternion.Euler(-yRotation, xRotation, 0);
            magicHandWeaponHolder.transform.rotation = Quaternion.Euler(0, xRotation, 0);
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

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //create ray from center of screen
            RaycastHit hit;
            Vector3 forceDirection = Camera.main.transform.forward;

            if (Physics.Raycast(ray, out hit, maxDistance, layersToHit))
            {
                forceDirection = (hit.point - currentWepHand.transform.position).normalized; //adjust hit point with weapon position before throw
            }

            Vector3 forceToAdd = forceDirection * throwForce + transform.up; //Calculate direction multiplied by force

            currentWepHand.transform.parent = null; //unparent weapon
            currentWepHand.AddComponent<Rigidbody>();
            thrownWepRB = currentWepHand.GetComponent<Rigidbody>();
            thrownWepRB.AddForce(forceToAdd, ForceMode.Impulse);

        }
       
    }

}
