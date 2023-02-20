using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public GameObject gameObject;
    private Rigidbody weaponRB;

    public void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Rigidbody>() != null && collision.gameObject.tag == "Wood")
        {
            weaponRB = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
            weaponRB.constraints = RigidbodyConstraints.FreezeAll;

        }
    }
}
