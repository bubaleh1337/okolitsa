using UnityEngine; // Gives access to Unity engine classes.

public class EpisodeStartTrigger : MonoBehaviour // Starts an episode when the player enters a trigger zone.
{
    [Header("Episode Reference")] // Groups episode reference settings in the Inspector.
    [SerializeField] private Episode01LightWentOutController episodeController; // Episode controller that will be started by this trigger.

    [Header("Player Detection")] // Groups player detection settings in the Inspector.
    [SerializeField] private Transform playerRoot; // Root transform of the player object used to identify player colliders.

    [Header("Trigger Behavior")] // Groups trigger behavior settings in the Inspector.
    [SerializeField] private bool startOnlyOnce = true; // Defines whether this trigger can start the episode only once.
    [SerializeField] private bool disableTriggerAfterStart = true; // Defines whether the trigger collider is disabled after the episode starts.

    [Header("Debug State")] // Shows internal trigger state in the Inspector.
    [SerializeField] private bool hasStarted; // Stores whether this trigger has already started the episode.

    private Collider triggerCollider; // Stores the trigger collider attached to this object.

    private void Awake() // Runs once when the object is loaded.
    {
        triggerCollider = GetComponent<Collider>(); // Finds the collider on this trigger object.

        ConfigureColliderAsTrigger(); // Ensures the collider is configured as a trigger.
    }

    private void OnTriggerEnter(Collider other) // Runs when another collider enters this trigger.
    {
        TryStartEpisodeFromCollider(other); // Tries to start the episode if the collider belongs to the player.
    }

    private void OnTriggerStay(Collider other) // Runs while another collider stays inside this trigger.
    {
        TryStartEpisodeFromCollider(other); // Also supports cases where the player starts already inside the trigger.
    }

    public void ResetTriggerForTesting() // Resets this trigger during testing.
    {
        hasStarted = false; // Allows the trigger to start the episode again.

        if (triggerCollider != null) // Checks if the trigger collider exists.
        {
            triggerCollider.enabled = true; // Re-enables the trigger collider.
        }
    }

    private void TryStartEpisodeFromCollider(Collider other) // Checks the collider and starts the episode if valid.
    {
        if (hasStarted && startOnlyOnce) // Checks if the episode already started and repeat starts are blocked.
        {
            return; // Stops safely to prevent duplicate starts.
        }

        if (!IsPlayerCollider(other)) // Checks if the collider does not belong to the player.
        {
            return; // Stops because this trigger should only react to the player.
        }

        StartEpisode(); // Starts the assigned episode.
    }

    private bool IsPlayerCollider(Collider other) // Checks whether a collider belongs to the assigned player.
    {
        if (other == null) // Checks if the collider reference is missing.
        {
            return false; // Not a valid player collider.
        }

        if (playerRoot == null) // Checks if no player root was assigned.
        {
            return other.CompareTag("Player"); // Falls back to Player tag detection.
        }

        Transform hitTransform = other.transform; // Stores the transform of the collider that entered the trigger.

        if (hitTransform == playerRoot) // Checks if the collider is directly on the player root.
        {
            return true; // The collider belongs to the player.
        }

        if (hitTransform.IsChildOf(playerRoot)) // Checks if the collider belongs to a child object of the player.
        {
            return true; // The collider belongs to the player hierarchy.
        }

        if (playerRoot.IsChildOf(hitTransform)) // Handles rare cases where the assigned root is below the collider transform.
        {
            return true; // The collider still belongs to the player hierarchy.
        }

        return false; // The collider does not belong to the player.
    }

    private void StartEpisode() // Starts the assigned episode controller.
    {
        if (episodeController == null) // Checks if the episode controller is missing.
        {
            Debug.LogWarning("EpisodeStartTrigger: Episode Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        hasStarted = true; // Stores that this trigger has started the episode.

        episodeController.StartEpisode(); // Starts Episode 01 through its public method.

        Debug.Log("EpisodeStartTrigger: Episode started."); // Logs the trigger activation.

        if (disableTriggerAfterStart && triggerCollider != null) // Checks if the trigger should be disabled after starting.
        {
            triggerCollider.enabled = false; // Disables the trigger so it cannot fire again.
        }
    }

    private void ConfigureColliderAsTrigger() // Ensures the attached collider behaves as a trigger.
    {
        if (triggerCollider == null) // Checks if no collider is attached.
        {
            Debug.LogWarning("EpisodeStartTrigger: No Collider found. Add a Box Collider and enable Is Trigger."); // Shows setup warning.
            return; // Stops safely.
        }

        triggerCollider.isTrigger = true; // Forces the collider to work as a trigger.
    }
}