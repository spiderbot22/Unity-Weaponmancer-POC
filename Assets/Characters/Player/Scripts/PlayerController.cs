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
        Debug.Log(context);
    }
    
    public void OnWalkUnarmed(InputAction.CallbackContext context)
    {

        if(_animator.GetBool("isWalkingUnarmed") == false) //toggle walking
        {
            _animator.SetBool("isWalkingUnarmed", true);
            _animator.SetBool("isJoggingUnarmed", false);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = 1.8f;
        } 
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = 4;
        }
        
    }

    public void OnSprintUnarmed(InputAction.CallbackContext context)
    {

        if(_animator.GetBool("isSprintingUnarmed") == false)
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", false);
            _animator.SetBool("isSprintingUnarmed", true);
        }
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
        }
        
    }


}
