using UnityEngine; // Gives access to Unity engine classes.

public class PlayerFootsteps : MonoBehaviour // Plays player footstep, jump, and landing sounds.
{
    [Header("References")] // Groups required component references in the Inspector.
    [SerializeField] private CharacterController characterController; // CharacterController used to check ground state.
    [SerializeField] private AudioSource audioSource; // AudioSource used to play movement sounds.
    [SerializeField] private SurfaceAudio defaultSurface; // Surface sounds used if no specific surface is found.

    [Header("Surface Detection")] // Groups ground and surface detection settings.
    [SerializeField] private LayerMask groundLayer = ~0; // Layers that can be detected as ground.
    [SerializeField] private float groundCheckDistance = 0.6f; // Distance used to detect the surface under the player.

    [Header("Step Timing")] // Groups footstep cadence settings.
    [SerializeField] private float walkStepInterval = 0.55f; // Time between walking steps on flat ground.
    [SerializeField] private float runStepInterval = 0.32f; // Time between running steps on flat ground.
    [SerializeField] private float stairDescentStepInterval = 0.75f; // Slower step interval used while descending stairs.
    [SerializeField] private float minimumHorizontalSpeedForSteps = 0.05f; // Minimum horizontal speed required to play footsteps.

    [Header("Landing Control")] // Groups landing sound filtering settings.
    [SerializeField] private float minimumAirTimeForLandingSound = 0.22f; // Minimum time in air before a landing sound is allowed.
    [SerializeField] private float minimumFallSpeedForLandingSound = 3.5f; // Minimum downward speed required for a landing sound.
    [SerializeField] private float stepDelayAfterLanding = 0.18f; // Short delay after landing before regular footsteps can play again.

    [Header("Input")] // Groups movement input settings.
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift; // Key used for running.
    [SerializeField] private KeyCode jumpKey = KeyCode.Space; // Key used for jumping.

    [Header("Pitch")] // Groups random pitch settings.
    [SerializeField] private float minPitch = 0.95f; // Lowest random pitch.
    [SerializeField] private float maxPitch = 1.05f; // Highest random pitch.

    private float nextStepTime; // Stores when the next footstep can play.
    private bool wasGrounded; // Stores grounded state from the previous frame.
    private bool isLeftFootNext = true; // Stores which foot should play next.
    private SurfaceAudio currentSurface; // Stores the surface currently under the player.

    private Vector3 previousPosition; // Stores the player's position from the previous frame.
    private float airborneTime; // Stores how long the player has been in the air.
    private float highestDownwardSpeedWhileAirborne; // Stores the strongest downward speed while airborne.

    private void Awake() // Runs once when the object is loaded.
    {
        if (characterController == null) // Checks if CharacterController was not assigned.
        {
            characterController = GetComponent<CharacterController>(); // Finds CharacterController on the player.
        }

        if (audioSource == null) // Checks if AudioSource was not assigned.
        {
            audioSource = GetComponent<AudioSource>(); // Finds AudioSource on the player.
        }

        if (characterController != null) // Checks if CharacterController exists.
        {
            wasGrounded = characterController.isGrounded; // Saves starting grounded state.
        }

        previousPosition = transform.position; // Saves the starting player position.
    }

    private void Update() // Runs once every frame.
    {
        if (characterController == null || audioSource == null) // Stops if required components are missing.
        {
            return; // Exits the method.
        }

        UpdateCurrentSurface(); // Detects the surface under the player.

        UpdateAirborneState(); // Tracks air time and downward movement.

        HandleJumpSound(); // Plays jump sound.

        HandleLandingSound(); // Plays landing sound only after real drops or jumps.

        HandleFootsteps(); // Plays walking or running footsteps.

        wasGrounded = characterController.isGrounded; // Saves grounded state for the next frame.

        previousPosition = transform.position; // Saves current position for the next frame.
    }

    private void UpdateCurrentSurface() // Finds the surface under the player.
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.2f; // Starts the ray slightly above the player feet.

        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore)) // Casts a ray downward.
        {
            SurfaceAudio surface = hit.collider.GetComponentInParent<SurfaceAudio>(); // Finds SurfaceAudio on the hit object or its parent.

            currentSurface = surface != null ? surface : defaultSurface; // Uses detected surface or fallback.
        }
        else // Runs if no surface was detected.
        {
            currentSurface = defaultSurface; // Uses fallback surface.
        }
    }

    private void UpdateAirborneState() // Tracks whether the player is really airborne.
    {
        float verticalDelta = transform.position.y - previousPosition.y; // Calculates vertical movement since the previous frame.

        float downwardSpeed = verticalDelta < 0f ? Mathf.Abs(verticalDelta) / Mathf.Max(Time.deltaTime, 0.0001f) : 0f; // Converts downward movement to speed.

        if (!characterController.isGrounded) // Checks if the player is currently in the air.
        {
            airborneTime += Time.deltaTime; // Increases airborne time.

            highestDownwardSpeedWhileAirborne = Mathf.Max(highestDownwardSpeedWhileAirborne, downwardSpeed); // Stores the strongest downward speed.
        }
    }

    private void HandleFootsteps() // Handles regular step sounds.
    {
        if (!characterController.isGrounded) // Checks if the player is not on the ground.
        {
            return; // Stops footsteps in the air.
        }

        if (!HasMovementInput()) // Checks if the player is not pressing movement keys.
        {
            return; // Stops if there is no movement input.
        }

        if (!HasEnoughHorizontalMovement()) // Checks if the player is not really moving horizontally.
        {
            return; // Stops fake footsteps caused by tiny controller movement.
        }

        if (Time.time < nextStepTime) // Checks if the next step is not ready yet.
        {
            return; // Waits for the next step time.
        }

        bool isRunning = Input.GetKey(runKey); // Checks if the run key is held.

        PlayStep(isRunning); // Plays a left or right footstep.

        float interval = GetCurrentStepInterval(isRunning); // Chooses step timing for flat ground or stair descent.

        nextStepTime = Time.time + interval; // Sets the next allowed step time.
    }

    private bool HasMovementInput() // Checks whether the player is pressing movement keys.
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Reads A and D input.

        float verticalInput = Input.GetAxisRaw("Vertical"); // Reads W and S input.

        return Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f; // Returns true if movement input exists.
    }

    private bool HasEnoughHorizontalMovement() // Checks whether the player is actually moving across the floor.
    {
        Vector3 positionDelta = transform.position - previousPosition; // Calculates movement since the previous frame.

        Vector3 horizontalDelta = new Vector3(positionDelta.x, 0f, positionDelta.z); // Removes vertical movement from the calculation.

        float horizontalSpeed = horizontalDelta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f); // Converts horizontal movement to speed.

        return horizontalSpeed >= minimumHorizontalSpeedForSteps; // Returns true only if horizontal movement is meaningful.
    }

    private float GetCurrentStepInterval(bool isRunning) // Returns the correct step interval for the current movement context.
    {
        if (IsDescendingStairsOrSlope()) // Checks if the player is moving downward while grounded.
        {
            return stairDescentStepInterval; // Uses a slower cadence for stair descent.
        }

        return isRunning ? runStepInterval : walkStepInterval; // Uses normal flat-ground walking or running cadence.
    }

    private bool IsDescendingStairsOrSlope() // Detects downward grounded movement such as stair descent.
    {
        if (!characterController.isGrounded) // Checks if the player is not grounded.
        {
            return false; // Not a grounded stair descent.
        }

        float verticalDelta = transform.position.y - previousPosition.y; // Calculates vertical movement since the previous frame.

        return verticalDelta < -0.005f; // Returns true if the player is moving down while still grounded.
    }

    private void PlayStep(bool isRunning) // Plays one footstep.
    {
        if (currentSurface == null) // Checks if no surface exists.
        {
            return; // Stops without error.
        }

        AudioClip clip = currentSurface.GetStepClip(isRunning, isLeftFootNext); // Gets a left or right footstep clip.

        PlayClip(clip); // Plays the selected clip.

        isLeftFootNext = !isLeftFootNext; // Switches to the other foot for the next step.
    }

    private void HandleJumpSound() // Plays jump sound.
    {
        if (characterController.isGrounded && Input.GetKeyDown(jumpKey)) // Checks if the player jumps while grounded.
        {
            if (currentSurface != null) // Checks if surface exists.
            {
                PlayClip(currentSurface.GetJumpClip()); // Plays jump sound.
            }
        }
    }

    private void HandleLandingSound() // Plays landing sound only for real landings.
    {
        if (wasGrounded || !characterController.isGrounded) // Checks if the player did not just land this frame.
        {
            return; // Stops if there is no landing event.
        }

        bool wasInAirLongEnough = airborneTime >= minimumAirTimeForLandingSound; // Checks if the player was airborne long enough.

        bool fellFastEnough = highestDownwardSpeedWhileAirborne >= minimumFallSpeedForLandingSound; // Checks if the fall was strong enough.

        airborneTime = 0f; // Resets airborne time after landing.

        highestDownwardSpeedWhileAirborne = 0f; // Resets tracked fall speed after landing.

        if (!wasInAirLongEnough || !fellFastEnough) // Checks if this was only a tiny stair contact.
        {
            return; // Suppresses landing spam on stairs.
        }

        if (currentSurface != null) // Checks if surface exists.
        {
            PlayClip(currentSurface.GetLandClip()); // Plays landing sound.
        }

        nextStepTime = Time.time + stepDelayAfterLanding; // Prevents a footstep from playing immediately after landing.
    }

    private void PlayClip(AudioClip clip) // Plays one sound clip.
    {
        if (clip == null) // Checks if there is no clip.
        {
            return; // Stops without error.
        }

        audioSource.pitch = Random.Range(minPitch, maxPitch); // Adds small pitch variation.

        audioSource.PlayOneShot(clip); // Plays the sound once.
    }
}