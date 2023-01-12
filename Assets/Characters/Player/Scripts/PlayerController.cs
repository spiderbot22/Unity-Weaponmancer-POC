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
    public float lookSensitivity = 5;
    private float lookSensitivityTemp;
    public Transform orientation;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask whatIsGround;
    bool grounded;
    
    private Vector3 moveDirection;
    private Vector3 moveDirectionSprint;
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
        lookSensitivityTemp = lookSensitivity;
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
        //_animator.SetFloat("velocityMagnitude", _rigidBody.velocity.magnitude);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void Move()
    {

        Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y);
        moveDirection = orientation.forward * moveVector.y + orientation.right * moveVector.x;
        moveDirectionSprint = orientation.forward * moveVector.y; //for disabling horizontal movement when sprinting

        /*Animating*/

        //grab forward/back and left/right velocity to pass to animator vars
        float velocityZ = Vector3.Dot(moveDirection.normalized, transform.forward);
        float velocityX = Vector3.Dot(moveDirection.normalized, transform.right);

        /*Movement*/

        if (movement.magnitude > 0 && _animator.GetBool("isSprintingUnarmed") == false)
        {
            _rigidBody.AddForce(moveDirection.normalized * moveSpeed * moveSpeed, ForceMode.Force);
        } 
        else if (movement.magnitude > 0 && _animator.GetBool("isSprintingUnarmed") && movement.z > 0)
        {
            _rigidBody.AddForce(moveDirectionSprint * moveSpeed * moveSpeed, ForceMode.Force);
        }
        
        //pass velocity to animator vars
        _animator.SetFloat("VelocityZ", velocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("VelocityX", velocityX, 0.1f, Time.deltaTime);
        _animator.SetFloat("backwardsCheck", movement.magnitude * movement.z, 0.1f, Time.deltaTime);
        _animator.SetFloat("velocityMagnitude", _rigidBody.velocity.magnitude);
    }

    private void Rotate()
    {
        
        mouseX = lookVector.x * Time.deltaTime * lookSensitivity;
        mouseY = lookVector.y * Time.deltaTime * lookSensitivity;

        yRotation += mouseX;
        xRotation += mouseX;

        _rigidBody.rotation = Quaternion.Euler(0, xRotation, 0);
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
        //if (isSprintingUnarmed is false, button is pressed but not released, moving forward)
        if (_animator.GetBool("isSprintingUnarmed") == false && context.performed && moveVector.y > 0)
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", false);
            _animator.SetBool("isSprintingUnarmed", true);
            moveSpeed = sprintSpeed;
            lookSensitivity = 0.5f;
        }
        else
        {
            _animator.SetBool("isWalkingUnarmed", false);
            _animator.SetBool("isJoggingUnarmed", true);
            _animator.SetBool("isSprintingUnarmed", false);
            moveSpeed = jogSpeed;
            lookSensitivity = lookSensitivityTemp; //return look sens to default
        }

    }
    public void OnLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    public void OnSheath(InputAction.CallbackContext context)
    {
        
        if(_animator.GetBool("inCombat") == false && context.performed)
        {
            _animator.SetTrigger("drawWeaponTrigger");
            _animator.SetBool("inCombat", true);
        } 
        else if(_animator.GetBool("inCombat") && context.performed)
        {
            _animator.SetTrigger("sheathWeaponTrigger");
            _animator.SetBool("inCombat", false);
        }
        
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
