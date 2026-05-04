using UnityEngine; 

public class ElevatorButton : MonoBehaviour, IInteractable // Makes this elevator button usable by PlayerInteraction.
{
    [SerializeField] private ElevatorController elevatorController; // Elevator controller that will move the player.

    [SerializeField] private Transform targetExitPoint; // Exit point where the player will appear.

    private void Awake() // Runs once when the object is loaded.
    {
        if (elevatorController == null) // Checks if the elevator controller was not assigned manually.
        {
            elevatorController = GetComponentInParent<ElevatorController>(); // Tries to find the controller in parent objects.
        }
    }

    public void Interact() // Runs when the player presses E while looking at this button.
    {
        if (elevatorController == null) // Checks if elevator controller is missing.
        {
            Debug.LogWarning("ElevatorButton: Elevator Controller is not assigned."); // Shows a warning in the Console.
            return; // Stops the method to avoid errors.
        }

        if (targetExitPoint == null) // Checks if target exit point is missing.
        {
            Debug.LogWarning("ElevatorButton: Target Exit Point is not assigned."); // Shows a warning in the Console.
            return; // Stops the method to avoid errors.
        }

        elevatorController.TravelTo(targetExitPoint); // Sends the player to the selected exit point.
    }
}