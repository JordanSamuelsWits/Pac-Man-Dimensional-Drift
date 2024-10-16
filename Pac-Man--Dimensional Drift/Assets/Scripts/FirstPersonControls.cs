using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /*
    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    // Private variables to store input values and the character controller
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 200f; // Speed at which the projectile is fired

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    //public Transform holdPosition; // Position where the picked-up object will be held
    //private GameObject heldObject; // Reference to the currently held object
    //private bool holdingGun = false;
    */
    

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

    /*
    We faced an issue whereby when consecutively pressing the keybinds for dashing and sliding, the actions would overlap
    causing the player to get stuck in the move speed of either action. So the idea is to fix this by checking if one is still active
    */


    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
    }

    private bool isPerformingAction = false; // To ensure that only one action (slide or dash) happens at a time
    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();
        
        // Enable the input actions
        playerInput.Player.Enable();

        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => Jump();

        //playerInput.Player.Shoot.performed += ctx => Shoot();

        //playerInput.Player.PickUp.performed += ctx => PickUpObject();

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

        //playerInput.Player.SpeedDash.performed += ctx => StartCoroutine(SpeedDash());

        // Subscribe to the interact input event
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

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);

        float currentSpeed = isSliding ? slideSpeed : moveSpeed; // Adjust speed during slide
    }

    public void LookAround() // Changing to logic to independently control the lookspeed of the mouse input and gamepd input
    {
        float LookX = 0f;
        float LookY = 0f;

        // We want to determine if the input is from the mouse or gamepad
        if (Mouse.current != null && Mouse.current.delta != null) // I swear there is a better way but I am not sure
        {
            // Get horizontal and vertical look inputs and adjust based on sensitivity
            LookX = lookInput.x * lookSpeed;
            LookY = lookInput.y * lookSpeed;
        }
        else if (Gamepad.current != null) // I swear there is a better way but I am not sure
        {
            LookX = lookInput.x * gamepadLookSpeed;
            LookY = lookInput.y * gamepadLookSpeed;
        }

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded && !isSliding)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }
            }

            else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }
        }
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float moveAmount = 5f; // The total distance the door will be raised
        float moveSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.left * moveAmount; // Adjusted for us

        // Continue sliding the door until it reaches the target height
        while (door.transform.position.x < endPosition.x)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }
    /*
    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;
            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);
    
        
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
        }
    }
    */


    public IEnumerator Slide()
    {
        
        isSliding = true; // Set sliding state to true
        canSlide = false; // Disable sliding temporarily to prevent spamming
        isPerformingAction = true; // Block other actions

        slideTime = slideDuration; // Reset the slide timer

        // Reduce character controller height for sliding
        characterController.height = slideHeight;

        // Temporarily increase the move speed for sliding
        float originalSpeed = moveSpeed;
        moveSpeed = slideSpeed;

        // Wait for the slide duration to complete
        yield return new WaitForSeconds(slideDuration);

        // Restore the original move speed and character controller height
        moveSpeed = originalSpeed;
        characterController.height = standingHeight;
        isSliding = false; // Set sliding state to false

        // Add a short cooldown before allowing the next slide
        yield return new WaitForSeconds(slideCooldown); // Cooldown duration can be adjusted
        canSlide = true; // Re-enable sliding
        isPerformingAction = false; // Now allows other actions
    }
    // We had a slight issue where if you spammed control (to slide), the character would
    // permanantly move with the slide speed and not the original move speed
    // So the new code is an attempt to prevent that and handle better edge cases by introducing a cool down period between slides
    
    //private bool canSlide = true; // To track if sliding is allowed
    private void SlideMovement()
    {
        if (isSliding && slideTime >= 0) // Updated the argument
        {
            slideTime -= Time.deltaTime; // Reduce the slide timer
        }
        else
        {
            isSliding = false; // End the slide
            //moveSpeed = moveSpeed != slideSpeed ? moveSpeed : moveSpeed; // This ensures the speed is restored
        }
    }

    private IEnumerator SpeedDash()
    {
        isDashing = true; // Set dashing state to true
        canDash = false; // Prevent further dashes until cooldown completes
        isPerformingAction = true; // Block other actions

        dashTime = dashDuration; // Reset the dash timer

        float originalSpeed = moveSpeed;
        moveSpeed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;
        isDashing = false; // Set dashing state to false

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        isPerformingAction = false; // Allow other actions
    }

    private void DashMovement()
    {
        if (dashTime >= 0)
        {
            dashTime -= Time.deltaTime; // Reduce the dash timer
        }
        else
        {
            isDashing = false; // End the dash
        }
    }
}
