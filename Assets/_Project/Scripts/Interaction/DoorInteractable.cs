using UnityEngine; // Gives access to Unity engine classes.

public sealed class DoorInteractable : MonoBehaviour, IInteractable // Makes a reusable hinged door interactable through PlayerInteraction.
{
    [Header("Door Rotation")] // Groups the physical door movement settings.
    [SerializeField] private float openAngle = 90f; // Defines the open angle; use a negative value for the opposite hinge direction.
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Defines the local axis used for door rotation.
    [SerializeField] private float rotationSpeed = 180f; // Defines how quickly the door rotates in degrees per second.

    [Header("Lock State")] // Groups reusable locking settings.
    [SerializeField] private bool startsLocked = false; // Defines whether the door begins the scene locked.
    [SerializeField] private bool logLockedInteractions = true; // Defines whether attempts to open a locked door are logged.

    [Header("Audio")] // Groups optional door and lock sounds.
    [SerializeField] private AudioSource audioSource; // Plays door movement and locked-handle sounds.
    [SerializeField] private AudioClip openClip; // Plays when the door begins opening.
    [SerializeField] private AudioClip closeClip; // Plays when the door begins closing.
    [SerializeField] private AudioClip lockedClip; // Plays when the player attempts to use a locked door.

    [Header("Runtime State")] // Shows the current door state during Play Mode.
    [SerializeField] private bool isOpen; // Stores whether the door is currently logically open.
    [SerializeField] private bool isLocked; // Stores whether interaction is currently blocked.

    private Quaternion closedRotation; // Stores the authored closed local rotation.
    private Quaternion openRotation; // Stores the calculated open local rotation.
    private Quaternion targetRotation; // Stores the rotation the door is currently moving toward.

    public bool IsOpen => isOpen; // Exposes the open state as read-only information.
    public bool IsLocked => isLocked; // Exposes the lock state as read-only information.

    private void Awake() // Runs when the door object is initialized.
    {
        closedRotation = transform.localRotation; // Saves the authored starting rotation as the closed state.

        openRotation = closedRotation // Begins from the saved closed rotation.
            * Quaternion.AngleAxis(openAngle, rotationAxis.normalized); // Calculates the local open rotation.

        isOpen = false; // Ensures the logical door state begins closed.
        isLocked = startsLocked; // Copies the authored starting lock state into runtime state.
        targetRotation = closedRotation; // Ensures the door begins moving toward the closed position.

        if (audioSource == null) // Checks whether the AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Attempts to find an AudioSource on the same object.
        }
    }

    private void Update() // Runs once per rendered frame.
    {
        transform.localRotation = Quaternion.RotateTowards( // Rotates the door smoothly toward its current target.
            transform.localRotation, // Uses the door's current local rotation.
            targetRotation, // Uses the currently requested open or closed rotation.
            rotationSpeed * Time.deltaTime); // Applies frame-rate-independent rotation speed.
    }

    public void Interact() // Runs when the player presses the interaction key while looking at the door.
    {
        if (isLocked) // Checks whether the door currently rejects normal interaction.
        {
            HandleLockedInteraction(); // Provides locked-door feedback without opening the door.
            return; // Stops normal open and close behaviour.
        }

        isOpen = !isOpen; // Changes the logical door state.

        targetRotation = isOpen // Checks whether the new state is open.
            ? openRotation // Uses the calculated open rotation.
            : closedRotation; // Uses the saved closed rotation.

        PlayDoorMovementSound(); // Plays the sound matching the new door state.
    }

    public void LockDoor() // Provides a parameterless method for Timeline signals or narrative systems.
    {
        SetLocked(true); // Applies the locked state.
    }

    public void UnlockDoor() // Provides a parameterless method for Timeline signals or narrative systems.
    {
        SetLocked(false); // Applies the unlocked state.
    }

    public void SetLocked(bool shouldBeLocked) // Sets the lock state explicitly.
    {
        if (isLocked == shouldBeLocked) // Checks whether the requested state is already active.
        {
            return; // Avoids unnecessary state changes.
        }

        isLocked = shouldBeLocked; // Stores the requested lock state.

        if (isLocked) // Checks whether the door has just become locked.
        {
            isOpen = false; // Forces the logical state to closed.
            targetRotation = closedRotation; // Moves the door back toward its closed rotation.
        }
    }

    public void ForceClose() // Provides an explicit method for future narrative staging.
    {
        isOpen = false; // Stores the closed logical state.
        targetRotation = closedRotation; // Moves the door toward the closed rotation.
    }

    private void HandleLockedInteraction() // Handles an attempt to use the door while it is locked.
    {
        isOpen = false; // Ensures the logical state remains closed.
        targetRotation = closedRotation; // Ensures the door remains at its closed rotation.

        PlayOneShot(lockedClip); // Plays the optional locked-handle or lock sound.

        if (logLockedInteractions) // Checks whether development logging is enabled.
        {
            Debug.Log($"{name}: door is locked.", this); // Reports the rejected interaction.
        }
    }

    private void PlayDoorMovementSound() // Plays the sound matching the open or closed state.
    {
        AudioClip selectedClip = isOpen // Checks whether the door is now open.
            ? openClip // Selects the opening sound.
            : closeClip; // Selects the closing sound.

        PlayOneShot(selectedClip); // Plays the selected optional clip.
    }

    private void PlayOneShot(AudioClip clip) // Plays one optional sound through the assigned AudioSource.
    {
        if (audioSource == null) // Checks whether no AudioSource is available.
        {
            return; // Allows the door to function silently.
        }

        if (clip == null) // Checks whether the requested clip is missing.
        {
            return; // Stops without producing an error.
        }

        audioSource.PlayOneShot(clip); // Plays the sound once.
    }

#if UNITY_EDITOR // Includes development commands only inside the Unity Editor.
    [ContextMenu("Debug/Lock Door")] // Adds a manual lock command to the component menu.
    private void DebugLockDoor() // Supports lock-state testing during Play Mode.
    {
        LockDoor(); // Locks and closes the door.
    }

    [ContextMenu("Debug/Unlock Door")] // Adds a manual unlock command to the component menu.
    private void DebugUnlockDoor() // Supports lock-state testing during Play Mode.
    {
        UnlockDoor(); // Unlocks the door.
    }

    [ContextMenu("Debug/Force Close Door")] // Adds a manual close command to the component menu.
    private void DebugForceCloseDoor() // Supports testing the authored closed position.
    {
        ForceClose(); // Closes the door without changing its lock state.
    }
#endif // Ends the Unity Editor-only section.
}