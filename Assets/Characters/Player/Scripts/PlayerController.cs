using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] LayerMask _aimLayerMask;
    [Header("Movement")]
    public float walkSpeed;
    public float jogSpeed;
    public float sprintSpeed;
    public float moveSpeed;
    public float groundDrag;
    public float lookSensitivity;
    public Transform orientation;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    bool grounded;
    
    private Vector3 moveDirection;
    private Vector2 moveVector;
    private Rigidbody _rigidBody;
    private Animator _animator;
    private Vector2 lookVector;
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private float yRotation;



    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; //remove cursor
    }

    // Update is called once per frame
    void Update()
    {
        DragHandler();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
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
            _rigidBody.AddForce(moveDirection.normalized * moveSpeed * moveSpeed, ForceMode.Force);

        }

        /*Animating*/

        //grab forward/back and left/right velocity to pass to animator vars
        float velocityZ = Vector3.Dot(moveDirection.normalized, transform.forward);
        float velocityX = Vector3.Dot(moveDirection.normalized, transform.right);

        //pass velocity to animator vars
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
         _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);

    }

    private void Rotate()
    {
        mouseX = lookVector.x * Time.deltaTime * lookSensitivity;
        mouseY = lookVector.y * Time.deltaTime * lookSensitivity;

        yRotation += mouseX;
        xRotation += mouseX;

         _rigidBody.rotation = Quaternion.Euler(0, xRotation, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, xRotation, 0), lookSensitivity * Time.deltaTime);
    }

    public void OnWalkUnarmed(InputAction.CallbackContext context)
    {

        if(_animator.GetBool("isWalkingUnarmed") == false) //toggle walking
        {
            _animator.SetBool("isWalkingUnarmed", true);
            _animator.SetBool("isJoggingUnarmed", false);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = walkSpeed;
        } 
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = jogSpeed;
        }
        
    }

    public void OnSprintUnarmed(InputAction.CallbackContext context)
    {

        if(_animator.GetBool("isSprintingUnarmed") == false)
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", false);
            _animator.SetBool("isSprintingUnarmed", true);
            moveSpeed = sprintSpeed;
        }
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = jogSpeed;
        }

    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    private void DragHandler()
    {
        /*Ground Check*/
        grounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, whatIsGround);

        /*Drag Handler*/
        if (grounded)
        {
            _rigidBody.drag = groundDrag;
        }
        else
        {
            _rigidBody.drag = 0;
        }
    }
}
