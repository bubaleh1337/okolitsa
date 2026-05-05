using UnityEngine; 

public class IndoorWindZoneTrigger : MonoBehaviour // Detects when the player is inside the building.
{
    [SerializeField] private WindAmbienceController windController; // Wind controller that changes wind volume.

    private void OnTriggerEnter(Collider other) // Runs when something enters this trigger.
    {
        if (other.CompareTag("Player")) // Checks if the player entered.
        {
            windController.SetInsideState(true); // Switches to indoor muffled wind.
        }
    }

    private void OnTriggerExit(Collider other) // Runs when something exits this trigger.
    {
        if (other.CompareTag("Player")) // Checks if the player exited.
        {
            windController.SetInsideState(false); // Switches to outdoor wind.
        }
    }
}