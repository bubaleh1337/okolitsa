using UnityEngine; // Gives access to Unity classes.

public class DoorInteractable : MonoBehaviour, IInteractable // Makes this door usable by PlayerInteraction.
{
    [SerializeField] private float openAngle = 90f; // Door open angle. Use 90 or -90 depending on hinge side.

    [SerializeField] private Vector3 rotationAxis = Vector3.up; // Local axis used for rotation. Usually Y axis.

    [SerializeField] private float rotationSpeed = 180f; // Door rotation speed in degrees per second.

    private Quaternion closedRotation; // Stores the door's closed rotation.

    private Quaternion openRotation; // Stores the door's open rotation.

    private Quaternion targetRotation; // Stores the rotation the door is moving toward.

    private bool isOpen; // Stores whether the door is open.

    private void Awake() // Runs once when the object is loaded.
    {
        closedRotation = transform.localRotation; // Saves the starting rotation as the closed state.

        openRotation = closedRotation * Quaternion.AngleAxis(openAngle, rotationAxis.normalized); // Calculates the open rotation.

        targetRotation = closedRotation; // Starts with the door closed.
    }

    private void Update() // Runs once every frame.
    {
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime); // Smoothly rotates the door toward the target rotation.
    }

    public void Interact() // Runs when the player presses E while looking at the door.
    {
        isOpen = !isOpen; // Switches the door state.

        targetRotation = isOpen ? openRotation : closedRotation; // Chooses open or closed target rotation.
    }
}