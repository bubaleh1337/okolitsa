using System.Collections; // Gives access to IEnumerator and coroutines.
using UnityEngine; // Gives access to Unity classes.

public class DoorInteractable : MonoBehaviour, IInteractable // Makes this door usable by PlayerInteraction.
{
    [Header("Door Rotation")] // Groups door rotation settings in the Inspector.
    [SerializeField] private float openAngle = 90f; // Door open angle. Use 90 or -90 depending on hinge side.
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Local axis used for rotation. Usually Y axis.
    [SerializeField] private float rotationSpeed = 180f; // Door rotation speed in degrees per second.

    [Header("Auto Close")] // Groups automatic door closing settings in the Inspector.
    [SerializeField] private bool autoClose = true; // Defines whether the door closes automatically after being opened.
    [SerializeField] private float autoCloseDelay = 6f; // Delay before the door closes automatically.
    [SerializeField] private bool resetAutoCloseTimerOnInteract = true; // Restarts the auto-close timer if the door is interacted with again while open.

    [Header("Audio")] // Groups door audio settings in the Inspector.
    [SerializeField] private AudioSource audioSource; // Audio source used to play door sounds.
    [SerializeField] private AudioClip openClip; // Sound played when the door opens.
    [SerializeField] private AudioClip closeClip; // Sound played when the door closes.

    private Quaternion closedRotation; // Stores the door's closed rotation.
    private Quaternion openRotation; // Stores the door's open rotation.
    private Quaternion targetRotation; // Stores the rotation the door is moving toward.

    private bool isOpen; // Stores whether the door is open.
    private Coroutine autoCloseRoutine; // Stores the currently running auto-close coroutine.

    private void Awake() // Runs once when the object is loaded.
    {
        closedRotation = transform.localRotation; // Saves the starting rotation as the closed state.

        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis.normalized); // Calculates the open rotation.

        targetRotation = closedRotation; // Starts with the door closed.

        if (audioSource == null) // Checks if AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Finds AudioSource on this object.
        }
    }

    private void Update() // Runs once every frame.
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime); // Smoothly rotates the door toward the target rotation.
    }

    public void Interact() // Runs when the player presses E while looking at the door.
    {
        if (isOpen) // Checks if the door is currently open.
        {
            CloseDoor(); // Closes the door manually.
            return; // Stops after manual close.
        }

        OpenDoor(); // Opens the door manually.
    }

    public void OpenDoor() // Opens the door and starts auto-close if enabled.
    {
        isOpen = true; // Stores that the door is open.

        targetRotation = openRotation; // Sets the target rotation to the open position.

        PlayDoorSound(); // Plays the open sound.

        StartAutoCloseTimer(); // Starts or restarts the auto-close timer.
    }

    public void CloseDoor() // Closes the door and stops auto-close.
    {
        isOpen = false; // Stores that the door is closed.

        targetRotation = closedRotation; // Sets the target rotation to the closed position.

        StopAutoCloseTimer(); // Stops the auto-close timer because the door is already closing.

        PlayDoorSound(); // Plays the close sound.
    }

    private void StartAutoCloseTimer() // Starts the automatic close countdown.
    {
        if (!autoClose) // Checks if automatic closing is disabled.
        {
            return; // Stops safely.
        }

        if (autoCloseRoutine != null && !resetAutoCloseTimerOnInteract) // Checks if a timer is already running and should not be restarted.
        {
            return; // Keeps the existing timer.
        }

        StopAutoCloseTimer(); // Stops any previous timer before starting a new one.

        autoCloseRoutine = StartCoroutine(AutoCloseRoutine()); // Starts a new auto-close timer.
    }

    private IEnumerator AutoCloseRoutine() // Waits and then closes the door automatically.
    {
        yield return new WaitForSeconds(autoCloseDelay); // Waits before closing the door.

        if (!isOpen) // Checks if the door was already closed manually.
        {
            autoCloseRoutine = null; // Clears the routine reference.
            yield break; // Stops the coroutine safely.
        }

        CloseDoor(); // Closes the door automatically.

        autoCloseRoutine = null; // Clears the routine reference.
    }

    private void StopAutoCloseTimer() // Stops the automatic close countdown if it is running.
    {
        if (autoCloseRoutine == null) // Checks if no timer is running.
        {
            return; // Stops safely.
        }

        StopCoroutine(autoCloseRoutine); // Stops the active timer.

        autoCloseRoutine = null; // Clears the routine reference.
    }

    private void PlayDoorSound() // Plays open or close sound.
    {
        if (audioSource == null) // Stops if there is no AudioSource.
        {
            return; // Exits the method.
        }

        AudioClip clipToPlay = isOpen ? openClip : closeClip; // Chooses sound based on door state.

        if (clipToPlay != null) // Checks if sound exists.
        {
            audioSource.PlayOneShot(clipToPlay); // Plays the selected sound once.
        }
    }
}