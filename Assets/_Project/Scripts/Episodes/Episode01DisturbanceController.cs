using UnityEngine; // Gives access to Unity engine classes.

public class Episode01DisturbanceController : MonoBehaviour // Controls one apartment disturbance event for Episode 01.
{
    [Header("Disturbance Object")] // Groups object movement settings in the Inspector.
    [SerializeField] private Transform objectToMove; // Object that will move when the disturbance happens.
    [SerializeField] private Vector3 localMoveOffset = new Vector3(0.4f, 0f, 0.2f); // Local offset applied to the object.
    [SerializeField] private Vector3 localRotationOffset = new Vector3(0f, 35f, 0f); // Local rotation change applied to the object.

    [Header("Audio")] // Groups audio settings in the Inspector.
    [SerializeField] private AudioSource audioSource; // AudioSource used to play the disturbance sound.
    [SerializeField] private AudioClip disturbanceClip; // Sound played when the disturbance happens.

    [Header("Debug State")] // Shows internal state in the Inspector.
    [SerializeField] private bool hasPlayed; // Stores whether this disturbance already happened.

    private Vector3 originalLocalPosition; // Stores the starting local position of the moved object.
    private Quaternion originalLocalRotation; // Stores the starting local rotation of the moved object.

    private void Awake() // Runs once when the object is loaded.
    {
        if (audioSource == null) // Checks if AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Tries to find AudioSource on this object.
        }

        CacheOriginalTransform(); // Saves the original object transform for controlled movement.
    }

    public void PlayDisturbance() // Runs the disturbance event once.
    {
        if (hasPlayed) // Checks if the disturbance already happened.
        {
            return; // Prevents repeating the same disturbance.
        }

        hasPlayed = true; // Marks this disturbance as completed.

        MoveObject(); // Moves or rotates the selected object.

        PlaySound(); // Plays the disturbance sound.

        Debug.Log("Episode 01: First apartment disturbance triggered."); // Logs the disturbance event.
    }

    public void ResetDisturbance() // Resets the disturbance for testing in Play Mode.
    {
        hasPlayed = false; // Allows the disturbance to be triggered again.

        RestoreOriginalTransform(); // Moves the object back to its original state.
    }

    private void CacheOriginalTransform() // Stores the original transform of the object.
    {
        if (objectToMove == null) // Checks if no object is assigned.
        {
            return; // Stops safely if there is nothing to cache.
        }

        originalLocalPosition = objectToMove.localPosition; // Saves the starting local position.
        originalLocalRotation = objectToMove.localRotation; // Saves the starting local rotation.
    }

    private void MoveObject() // Applies the disturbance movement to the object.
    {
        if (objectToMove == null) // Checks if no object is assigned.
        {
            Debug.LogWarning("Episode01DisturbanceController: Object To Move is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        objectToMove.localPosition = originalLocalPosition + localMoveOffset; // Moves the object from its original position.
        objectToMove.localRotation = originalLocalRotation * Quaternion.Euler(localRotationOffset); // Rotates the object from its original rotation.
    }

    private void RestoreOriginalTransform() // Restores the object to its cached starting transform.
    {
        if (objectToMove == null) // Checks if no object is assigned.
        {
            return; // Stops safely if there is nothing to restore.
        }

        objectToMove.localPosition = originalLocalPosition; // Restores original local position.
        objectToMove.localRotation = originalLocalRotation; // Restores original local rotation.
    }

    private void PlaySound() // Plays the disturbance sound if available.
    {
        if (audioSource == null) // Checks if there is no AudioSource.
        {
            return; // Stops safely if audio cannot be played.
        }

        if (disturbanceClip == null) // Checks if no sound clip is assigned.
        {
            return; // Stops safely if there is no clip yet.
        }

        audioSource.PlayOneShot(disturbanceClip); // Plays the disturbance sound once.
    }
}