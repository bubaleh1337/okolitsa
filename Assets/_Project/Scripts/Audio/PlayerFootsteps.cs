using UnityEngine; 

public class PlayerFootsteps : MonoBehaviour // Plays player footstep, jump, and landing sounds.
{
    [SerializeField] private CharacterController characterController; // CharacterController used to check ground state.

    [SerializeField] private AudioSource audioSource; // AudioSource used to play movement sounds.

    [SerializeField] private SurfaceAudio defaultSurface; // Surface sounds used if no specific surface is found.

    [SerializeField] private LayerMask groundLayer = ~0; // Layers that can be detected as ground.

    [SerializeField] private float groundCheckDistance = 1.4f; // Distance used to detect the surface under the player.

    [SerializeField] private float walkStepInterval = 0.55f; // Time between walking steps.

    [SerializeField] private float runStepInterval = 0.32f; // Time between running steps.

    [SerializeField] private KeyCode runKey = KeyCode.LeftShift; // Key used for running.

    [SerializeField] private KeyCode jumpKey = KeyCode.Space; // Key used for jumping.

    [SerializeField] private float minPitch = 0.95f; // Lowest random pitch.

    [SerializeField] private float maxPitch = 1.05f; // Highest random pitch.

    private float nextStepTime; // Stores when the next footstep can play.

    private bool wasGrounded; // Stores grounded state from the previous frame.

    private bool isLeftFootNext = true; // Stores which foot should play next.

    private SurfaceAudio currentSurface; // Stores the surface currently under the player.

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
    }

    private void Update() // Runs once every frame.
    {
        if (characterController == null || audioSource == null) // Stops if required components are missing.
        {
            return; // Exits the method.
        }

        UpdateCurrentSurface(); // Detects the surface under the player.

        HandleJumpSound(); // Plays jump sound.

        HandleLandingSound(); // Plays landing sound.

        HandleFootsteps(); // Plays walking or running footsteps.

        wasGrounded = characterController.isGrounded; // Saves grounded state for the next frame.
    }

    private void UpdateCurrentSurface() // Finds the surface under the player.
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.2f; // Starts the ray slightly above the player.

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

    private void HandleFootsteps() // Handles regular step sounds.
    {
        if (!characterController.isGrounded) // Checks if the player is not on the ground.
        {
            return; // Stops footsteps in the air.
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Reads A and D input.

        float verticalInput = Input.GetAxisRaw("Vertical"); // Reads W and S input.

        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f; // Checks if the player is pressing movement keys.

        if (!isMoving) // Checks if the player is not moving.
        {
            return; // Stops if there is no movement input.
        }

        if (Time.time < nextStepTime) // Checks if the next step is not ready yet.
        {
            return; // Waits for the next step time.
        }

        bool isRunning = Input.GetKey(runKey); // Checks if the run key is held.

        PlayStep(isRunning); // Plays a left or right footstep.

        float interval = isRunning ? runStepInterval : walkStepInterval; // Chooses step timing.

        nextStepTime = Time.time + interval; // Sets the next allowed step time.
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

    private void HandleLandingSound() // Plays landing sound.
    {
        if (!wasGrounded && characterController.isGrounded) // Checks if the player just landed.
        {
            if (currentSurface != null) // Checks if surface exists.
            {
                PlayClip(currentSurface.GetLandClip()); // Plays landing sound.
            }
        }
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