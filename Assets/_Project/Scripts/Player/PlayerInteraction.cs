using UnityEngine; // Gives access to Unity engine classes.

public interface IInteractable // Defines the common interaction contract for world objects.
{
    void Interact(); // Requires every interactable object to provide interaction behavior.
}

public class PlayerInteraction : MonoBehaviour // Handles player interaction detection, input, and prompt visibility.
{
    [Header("Interaction Ray")] // Groups interaction-ray settings in the Inspector.
    [SerializeField] private Camera playerCamera; // Camera used as the origin of the interaction ray.
    [SerializeField] private float interactDistance = 2.5f; // Maximum interaction distance.
    [SerializeField] private LayerMask interactableLayer; // Layers that can contain interactable objects.

    [Header("Input")] // Groups interaction-input settings in the Inspector.
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key used to interact with a targeted object.
    [SerializeField] private bool interactionEnabled = true; // Defines whether interaction input and detection are currently allowed.

    [Header("UI")] // Groups interaction UI references in the Inspector.
    [SerializeField] private InteractionPromptController interactionPromptController; // Controls the interaction prompt shown on screen.

    private IInteractable currentInteractable; // Stores the interactable object currently under the crosshair.

    public bool IsInteractionEnabled => interactionEnabled; // Exposes the current interaction permission as read-only state.

    private void Update() // Runs once per frame.
    {
        if (!interactionEnabled) // Checks whether interaction is currently blocked.
        {
            ClearInteractionState(); // Clears the target and hides the prompt.
            return; // Stops interaction processing.
        }

        UpdateCurrentInteractable(); // Detects the object currently targeted by the player.

        UpdateInteractionPrompt(); // Shows or hides the interaction prompt.

        if (Input.GetKeyDown(interactKey)) // Checks whether the interaction key was pressed.
        {
            TryInteractWithCurrentTarget(); // Attempts interaction with the targeted object.
        }
    }

    private void UpdateCurrentInteractable() // Detects an interactable object in front of the player.
    {
        currentInteractable = null; // Clears the previous target before the new raycast.

        if (playerCamera == null) // Checks whether the camera reference is missing.
        {
            return; // Stops safely because a ray cannot be created.
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // Creates a forward ray from the camera.

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer, QueryTriggerInteraction.Ignore)) // Checks whether the ray hit a valid layer.
        {
            return; // Stops because no interaction target was found.
        }

        currentInteractable = hit.collider.GetComponentInParent<IInteractable>(); // Searches the hit object hierarchy for IInteractable.
    }

    private void UpdateInteractionPrompt() // Updates prompt visibility based on the current target.
    {
        if (interactionPromptController == null) // Checks whether the prompt controller is missing.
        {
            return; // Stops safely because the UI reference is optional.
        }

        if (currentInteractable == null) // Checks whether there is no valid target.
        {
            interactionPromptController.HidePrompt(); // Hides the interaction prompt.
            return; // Stops after updating the UI.
        }

        interactionPromptController.ShowDefaultPrompt(); // Shows the default interaction prompt.
    }

    private void TryInteractWithCurrentTarget() // Executes interaction on the targeted object.
    {
        if (playerCamera == null) // Checks whether the camera reference is missing.
        {
            Debug.LogWarning("PlayerInteraction: Player Camera is not assigned."); // Reports the missing reference.
            return; // Stops safely.
        }

        if (currentInteractable == null) // Checks whether there is no current interaction target.
        {
            return; // Stops because there is nothing to interact with.
        }

        currentInteractable.Interact(); // Executes the target object's interaction behavior.
    }

    public void SetInteractionEnabled(bool isEnabled) // Changes whether interaction is currently allowed.
    {
        interactionEnabled = isEnabled; // Stores the requested interaction state.

        if (!interactionEnabled) // Checks whether interaction has just been blocked.
        {
            ClearInteractionState(); // Immediately removes any visible prompt and cached target.
        }
    }

    private void ClearInteractionState() // Clears interaction targeting and prompt state.
    {
        currentInteractable = null; // Removes the cached interaction target.

        if (interactionPromptController != null) // Checks whether the prompt controller exists.
        {
            interactionPromptController.HidePrompt(); // Hides the prompt immediately.
        }
    }

    private void OnDisable() // Runs when this component or GameObject is disabled.
    {
        ClearInteractionState(); // Prevents a prompt from remaining visible after the component is disabled.
    }

    private void OnDrawGizmosSelected() // Draws the interaction ray in the Scene view.
    {
        if (playerCamera == null) // Checks whether the camera reference is missing.
        {
            return; // Stops drawing safely.
        }

        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance); // Draws the interaction range.
    }
}