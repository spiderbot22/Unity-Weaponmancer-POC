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
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontal = moveVector.x;
        float vertical = moveVector.y;

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        /*Movement*/

        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        /*Animating*/

        //grab forward/back and left/right velocity to pass to animator vars
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        //pass velocity to animator vars
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime); //("param name", param value, smoothing time, delta time)
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

    }

    public void InputRead(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

}
