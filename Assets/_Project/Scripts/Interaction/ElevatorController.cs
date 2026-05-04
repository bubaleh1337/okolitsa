using System.Collections; 
using UnityEngine; 

public class ElevatorController : MonoBehaviour // Controls fake elevator travel between floors.
{
    [SerializeField] private Transform player; // Player object that will be moved.

    [SerializeField] private CharacterController playerCharacterController; // Player CharacterController that must be disabled during teleport.

    [SerializeField] private float travelDelay = 2f; // Delay before moving the player to another floor.

    private bool isMoving; // Prevents starting elevator travel twice at the same time.

    public void TravelTo(Transform exitPoint) // Starts elevator travel to the selected exit point.
    {
        if (isMoving) // Checks if the elevator is already moving.
        {
            return; // Stops the method if travel is already active.
        }

        if (exitPoint == null) // Checks if target exit point is missing.
        {
            Debug.LogWarning("ElevatorController: Exit point is not assigned."); // Shows a warning in the Console.
            return; // Stops the method to avoid errors.
        }

        StartCoroutine(TravelRoutine(exitPoint)); // Starts delayed elevator travel.
    }

    private IEnumerator TravelRoutine(Transform exitPoint) // Handles the fake elevator travel process.
    {
        isMoving = true; // Marks the elevator as moving.

        yield return new WaitForSeconds(travelDelay); // Waits before teleporting the player.

        if (player == null) // Checks if player reference is missing.
        {
            Debug.LogWarning("ElevatorController: Player is not assigned."); // Shows a warning in the Console.
            isMoving = false; // Allows the elevator to be used again.
            yield break; // Stops the coroutine.
        }

        if (playerCharacterController != null) // Checks if CharacterController exists.
        {
            playerCharacterController.enabled = false; // Disables CharacterController before teleport.
        }

        player.position = exitPoint.position; // Moves the player to the exit point position.

        player.rotation = exitPoint.rotation; // Rotates the player to match the exit point direction.

        if (playerCharacterController != null) // Checks if CharacterController exists.
        {
            playerCharacterController.enabled = true; // Enables CharacterController after teleport.
        }

        isMoving = false; // Marks the elevator as ready again.
    }
}