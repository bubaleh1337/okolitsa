using UnityEngine; // Gives access to Unity classes.

public class DoorInteractable : MonoBehaviour, IInteractable // Makes this door usable by PlayerInteraction.
{
    [SerializeField] private float openAngle = 90f; // Door open angle. Use 90 or -90 depending on hinge side.
    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Local axis used for rotation. Usually Y axis.
    [SerializeField] private float rotationSpeed = 180f; // Door rotation speed in degrees per second.

    [SerializeField] private AudioSource audioSource; // Audio source used to play door sounds.
    [SerializeField] private AudioClip openClip; // Sound played when the door opens.
    [SerializeField] private AudioClip closeClip; // Sound played when the door closes.

    private Quaternion closedRotation; // Stores the door's closed rotation.
    private Quaternion openRotation; // Stores the door's open rotation.
    private Quaternion targetRotation; // Stores the rotation the door is moving toward.

    private bool isOpen; // Stores whether the door is open.

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
        isOpen = !isOpen; // Switches the door state.

        targetRotation = isOpen ? openRotation : closedRotation; // Chooses open or closed target rotation.

        PlayDoorSound(); // Plays the correct door sound.
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