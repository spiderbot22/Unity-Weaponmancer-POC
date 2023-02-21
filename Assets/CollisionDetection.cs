using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject weapon;
    private Rigidbody weaponRB;

    public void OnCollisionEnter(Collision collision)
    {
        if (weapon.GetComponent<Rigidbody>() != null && collision.gameObject.tag == "Wood")
        {
            weaponRB = weapon.GetComponent<Rigidbody>();
            weaponRB.isKinematic = true;
        }
    }
}
