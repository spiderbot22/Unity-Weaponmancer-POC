using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;

    [SerializeField]
    private float walkSpeed = 1.8f;

    [SerializeField]
    private float jogSpeed = 5;

    [SerializeField]
    private float sprintSpeed = 10;

    [SerializeField]
    private float lookSensitivity = 5;

    [SerializeField]
    private float jumpHeight = 10;

    [SerializeField]
    private float gravity = 9.81f;

    private float verticalVelocity;
    private Vector2 moveVector; //indicates movement direction
    private Vector2 lookVector;
    private Vector3 rotation;
    private CharacterController characterController; //character controller reference
    private Animator animator; //animator reference



    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>(); //take in input
        Debug.Log(moveVector.magnitude);

    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded && context.performed) //if on ground and jump button pressed
        {
            //animator.Play("Unarmed Jump");
        }
    }

    public void OnWalk(InputAction.CallbackContext context) //toggle walking speed
    {
        if (moveSpeed != walkSpeed)
        {
            moveSpeed = walkSpeed;
        } 

        else
        {
            moveSpeed = jogSpeed;
        }
    }

    public void OnSprint(InputAction.CallbackContext context) //sprint speed on input hold, jog speed on release
    {

        //if button pressed and not sprinting already
        if (context.started && moveSpeed != sprintSpeed)
        {
            moveSpeed = sprintSpeed;
        }

        //if button released and sprinting already
        if (context.canceled && moveSpeed == sprintSpeed)
        {
            moveSpeed = jogSpeed;
        }

    }
    
    private void Jump()
    {
        //verticalVelocity = Mathf.Sqrt(jumpHeight * gravity);
    }

    private void Rotate() //player rotatation based on mouse movement
    {
        rotation.y += lookVector.x * lookSensitivity * Time.deltaTime;
        transform.localEulerAngles = rotation;
    }

    private void Move()
    {
        verticalVelocity += -gravity * Time.deltaTime;

        if(characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -0.1f*gravity*Time.deltaTime; 
        }

        Vector3 move = transform.right * moveVector.x + transform.forward * moveVector.y + transform.up*verticalVelocity; //calc movement
        characterController.Move(move * moveSpeed * Time.deltaTime); //call move method on char controller
        Debug.Log(characterController.velocity);
    }

    

}
