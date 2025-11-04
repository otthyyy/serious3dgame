using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 7.2f;
    [SerializeField] private float jumpForce = 4.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float airControl = 0.3f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundMask = ~0;

    [Header("FOV")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float baseFOV = 90f;
    [SerializeField] private float sprintFOVKick = 3f;
    [SerializeField] private float fovTransitionSpeed = 8f;

    [Header("Audio")]
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip landSound;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private float sprintFootstepInterval = 0.3f;

    private CharacterController controller;
    private AudioSource audioSource;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private bool isSprinting;
    private float footstepTimer;
    private float targetFOV;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        
        inputActions = new PlayerInputActions();
        
        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
        
        if (playerCamera != null)
        {
            targetFOV = baseFOV;
            playerCamera.fieldOfView = baseFOV;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Sprint.performed += OnSprintPerformed;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        inputActions.Player.Disable();
    }

    private void OnDestroy()
    {
        inputActions?.Dispose();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y = jumpForce;
            PlaySound(jumpSound);
        }
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isSprinting = false;
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleFOV();
        HandleFootsteps();
        
        if (!wasGrounded && isGrounded && velocity.y < -2f)
        {
            PlaySound(landSound);
        }
        
        wasGrounded = isGrounded;
    }

    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 
            controller.height / 2 + groundCheckDistance, groundMask);
    }

    private void HandleMovement()
    {
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        if (isGrounded)
        {
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
            
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
        else
        {
            controller.Move(moveDirection * currentSpeed * airControl * Time.deltaTime);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleFOV()
    {
        if (playerCamera == null) return;
        
        targetFOV = isSprinting ? baseFOV + sprintFOVKick : baseFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, 
            Time.deltaTime * fovTransitionSpeed);
    }

    private void HandleFootsteps()
    {
        if (!isGrounded || moveInput.magnitude < 0.1f) return;
        
        float interval = isSprinting ? sprintFootstepInterval : footstepInterval;
        footstepTimer += Time.deltaTime;
        
        if (footstepTimer >= interval)
        {
            footstepTimer = 0f;
            PlayFootstep();
        }
    }

    private void PlayFootstep()
    {
        if (footstepSounds != null && footstepSounds.Length > 0)
        {
            AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
            PlaySound(clip);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool IsGrounded => isGrounded;
    public bool IsSprinting => isSprinting;
    public float CurrentSpeed => isSprinting ? sprintSpeed : walkSpeed;
}
