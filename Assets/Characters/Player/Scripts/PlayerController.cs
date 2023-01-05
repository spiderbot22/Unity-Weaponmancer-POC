using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask _aimLayerMask;
    public Transform orientation;
    Vector3 moveDirection;
    Vector2 moveVector;
    Rigidbody _rigidBody;
    Animator _animator;
    private Vector2 lookVector;
    private Vector3 rotation;
    private float lookSensitivity = 5;


    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void Move()
    {
        float horizontal = moveVector.x;
        float vertical = moveVector.y;
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        /*Movement*/

        if (movement.magnitude > 0)
        {
            movement.Normalize();
            movement *= moveSpeed * Time.deltaTime;
            //_rigidBody.AddForce(movement * moveSpeed * 30f, ForceMode.Force);
            _rigidBody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        }

        /*Animating*/

        //grab forward/back and left/right velocity to pass to animator vars
        //float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        //float velocityX = Vector3.Dot(movement.normalized, transform.right);
        float velocityZ = Vector3.Dot(moveDirection.normalized, transform.forward);
        float velocityX = Vector3.Dot(moveDirection.normalized, transform.right);

        //Quaternion newDirection = Quaternion.LookRotation(movement);

        //pass velocity to animator vars
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
         _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

        /*Rotation*/

        transform.Rotate(Vector3.up, lookVector.x * lookSensitivity * Time.deltaTime);



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
            moveSpeed = 8;
        }
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = 4;
        }

    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

}
