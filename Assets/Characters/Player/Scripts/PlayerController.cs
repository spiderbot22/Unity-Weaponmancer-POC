using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    Vector2 moveVector;
    Rigidbody rigidBody;
    Animator _animator;



    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = moveVector.x;
        float vertical = moveVector.y;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        //Moving
        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }

    public void InputRead(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        Debug.Log(moveVector);
    }

}
