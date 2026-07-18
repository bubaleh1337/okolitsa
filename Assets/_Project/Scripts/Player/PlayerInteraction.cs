using UnityEngine; // Gives access to Unity engine classes.

public interface IInteractable // Defines a simple rule for objects that can be interacted with.
{
    void Interact(); // Any interactable object must have this method.
}

public class PlayerInteraction : MonoBehaviour // Handles player interaction with world objects.
{
    [Header("Interaction Ray")] // Groups interaction ray settings in the Inspector.
    [SerializeField] private Camera playerCamera; // Camera used to shoot the interaction ray from the player's view.
    [SerializeField] private float interactDistance = 2.5f; // Maximum distance where the player can interact.
    [SerializeField] private LayerMask interactableLayer; // Layers that can be detected by the interaction ray.

    [Header("Input")] // Groups input settings in the Inspector.
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key used to interact with objects.

    [Header("UI")] // Groups interaction UI settings in the Inspector.
    [SerializeField] private InteractionPromptController interactionPromptController; // UI prompt shown when looking at an interactable object.

    private IInteractable currentInteractable; // Stores the interactable object currently under the player's crosshair.

    private void Update() // Runs once every frame.
    {
        UpdateCurrentInteractable(); // Checks what the player is currently looking at.

        UpdateInteractionPrompt(); // Shows or hides the interaction prompt based on the current target.

        if (Input.GetKeyDown(interactKey)) // Checks if the interaction key was pressed this frame.
        {
            TryInteractWithCurrentTarget(); // Tries to interact with the current target.
        }
    }

    private void UpdateCurrentInteractable() // Finds the interactable object currently in front of the player.
    {
        currentInteractable = null; // Clears the previous target before checking the new frame.

        if (playerCamera == null) // Checks if no camera is assigned.
        {
            return; // Stops safely without spamming warnings every frame.
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // Creates a ray from the camera forward.

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer, QueryTriggerInteraction.Ignore)) // Checks if no interactable object was hit.
        {
            return; // Stops because there is no valid interaction target.
        }

        currentInteractable = hit.collider.GetComponentInParent<IInteractable>(); // Searches for an interactable component on the hit object or its parents.
    }

    private void UpdateInteractionPrompt() // Updates the interaction prompt visibility.
    {
        if (interactionPromptController == null) // Checks if no prompt UI is assigned.
        {
            return; // Stops safely because the prompt is optional.
        }

        if (currentInteractable == null) // Checks if there is no interactable object in front of the player.
        {
            interactionPromptController.HidePrompt(); // Hides the prompt when nothing can be used.
            return; // Stops after hiding the prompt.
        }

        interactionPromptController.ShowDefaultPrompt(); // Shows the prompt when an interactable object is targeted.
    }

    private void TryInteractWithCurrentTarget() // Interacts with the current target if possible.
    {
        if (playerCamera == null) // Checks if no camera is assigned.
        {
            Debug.LogWarning("PlayerInteraction: Player Camera is not assigned."); // Shows a warning in the Console.
            return; // Stops safely to avoid errors.
        }

        if (currentInteractable == null) // Checks if there is no interactable object in front of the player.
        {
            return; // Stops because there is nothing to interact with.
        }

        currentInteractable.Interact(); // Runs the interaction logic on the found object.
    }

    private void OnDrawGizmosSelected() // Draws a helper line in the Scene view when the object is selected.
    {
        if (playerCamera == null) // Stops drawing if no camera is assigned.
        {
            return; // Exits the method.
        }

        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance); // Draws the interaction ray.
    }
}