using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class FirstPersonControls : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
    // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("LOOKSPEED SETTINGS")]
    [Space(5)]
    public float lookSpeed; // Sensitivity of the camera movement
    public float mouseLookSpeed = 1.0f;     // Mouse sensitivity
    public float gamepadLookSpeed = 1.0f;   // Gamepad sensitivity

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color
    public float pickUpRange = 3f; // Range within which objects can be interacted with

    [Header("SLIDING SETTINGS")]
    [Space(5)]
    public float slideHeight = 0.5f;
    public float standingHeight = 2f;
    public float slideSpeed = 10f; // Speed at which the player slides
    public float slideDuration = 0.5f; // Duration of the slide
    public float slideCooldown = 1f; // Adjustable cooldown duration for sliding
    private bool isSliding = false; // Whether the player is currently sliding
    private float slideTime = 0f; // Timer to track the slide duration
    private bool canSlide = true; // To control the cooldown between slides

    [Header("SPEEDASH SETTINGS")]
    [Space(5)]
    public float dashSpeed = 20f; // Speed at which the player dashes
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 1f; // Adjustable cooldown duration for dashing
    private bool isDashing = false; // Whether the player is currently dashing
    private float dashTime = 0f; // Timer to track the dash duration
    private bool canDash = true; // To control the cooldown between dashes

    [Header("COMBAT SETTINGS")]
    [Space(5)]
    public float attackRange = 5f; // Range for punch and kick attacks
    public int punchDamage = 10;   // Damage dealt by punch
    public int kickDamage = 20;    // Damage dealt by kick
    public string[] enemyTags;     // Tags of the enemies the player can attack

    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private bool isPerformingAction = false; // To ensure that only one action (slide or dash) happens at a time
    private void OnEnable()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();

        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => Jump();

        // Combat Input Bindings for Punch and Kick
        playerInput.Player.Punch.performed += ctx =>
        {
            Debug.Log("Punch action triggered");
            PerformPunch();
        };
        playerInput.Player.Kick.performed += ctx =>
        {
            Debug.Log("Kick action triggered");
            PerformKick();
        };

        // Updated slide logic
        playerInput.Player.Slide.performed += ctx =>
        {
            if (canSlide && !isPerformingAction) StartCoroutine(Slide()); // Only start slide if it's allowed
        };

        // Updated dash logic
        playerInput.Player.SpeedDash.performed += ctx =>
        {
            if (canDash && !isPerformingAction) StartCoroutine(SpeedDash());
        };

        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch
    }

    private void Update()
    {
        Move();
        LookAround();
        ApplyGravity();

        // Prevent sliding if jumping
        if (!characterController.isGrounded)
        {
            isSliding = false; // Ensure the player stops sliding when in the air
        }
    }

    // Movement
    public void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);
        characterController.Move(move * moveSpeed * Time.deltaTime);
    }

    // LookAround
    public void LookAround()
    {
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        transform.Rotate(0, LookX, 0);

        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    // Apply Gravity
    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // Jump
    public void Jump()
    {
        if (characterController.isGrounded && !isSliding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Combat Methods: Punch and Kick
    private void PerformPunch()
    {
        Debug.Log("Punch performed!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, attackRange))
        {
            // Check if the hit object has one of the tags in the enemyTags list
            foreach (string tag in enemyTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        Debug.Log("Enemy hit with punch!");
                        enemyHealth.TakeDamage(punchDamage);  // Deal damage to the enemy
                    }
                    return; // Exit the loop once an enemy is found and damaged
                }
            }
        }
    }

    private void PerformKick()
    {
        Debug.Log("Kick performed!");
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, attackRange))
        {
            // Check if the hit object has one of the tags in the enemyTags list
            foreach (string tag in enemyTags)
            {
                if (hit.collider.CompareTag(tag))
                {
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth != null)
                    {
                        Debug.Log("Enemy hit with kick!");
                        enemyHealth.TakeDamage(kickDamage);  // Deal damage to the enemy
                    }
                    return; // Exit the loop once an enemy is found and damaged
                }
            }
        }
    }

    // Interact
    public void Interact()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch"))
            {
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color;
                    }
                }
            }
            else if (hit.collider.CompareTag("Door"))
            {
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float moveAmount = 5f;
        float moveSpeed = 2f;
        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = startPosition + Vector3.left * moveAmount;

        while (door.transform.position.x < endPosition.x)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // Slide
    public IEnumerator Slide()
    {
        isSliding = true;
        canSlide = false;
        isPerformingAction = true;

        slideTime = slideDuration;

        characterController.height = slideHeight;
        float originalSpeed = moveSpeed;
        moveSpeed = slideSpeed;

        yield return new WaitForSeconds(slideTime);

        characterController.height = standingHeight;
        moveSpeed = originalSpeed;

        yield return new WaitForSeconds(slideCooldown);

        canSlide = true;
        isPerformingAction = false;
    }

    // Dash
    public IEnumerator SpeedDash()
    {
        isDashing = true;
        canDash = false;
        isPerformingAction = true;

        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
        isPerformingAction = false;
    }
}
