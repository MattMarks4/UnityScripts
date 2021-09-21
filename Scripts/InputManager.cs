using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocomotion playerLocomotion;

    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
    public bool runInput;
    public bool sprintInput;
    public bool jumpInput;
    public bool isMoving;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Run.performed += i => runInput = true;
            playerControls.PlayerMovement.Run.canceled += i => runInput = false;
            playerControls.PlayerMovement.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprintInput = false;
            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;


        }

        playerControls.Enable();
    }

    private void OnDisable()
        {
            playerControls.Disable();
        } 

    private void HandleMovementInput()
    {
        if(movementInput != Vector2.zero)
        {
            isMoving = true;
        }       
        else if(movementInput == Vector2.zero)
        {
            isMoving = false;
        }
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.MovementAnimations(0, moveAmount, isMoving, runInput, sprintInput);
       
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.HandleJump();
        }
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleJumpInput();
    }
    
}
