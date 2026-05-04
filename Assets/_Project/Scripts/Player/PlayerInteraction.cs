using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IInteractable // Defines a simple rule for objects that can be interacted with.
    {
        void Interact(); // Any interactable object must have this method.
    }

    public class PlayerInteraction : MonoBehaviour // Handles player interaction with world objects.
    {
        [SerializeField] private Camera playerCamera; // Camera used to shoot the interaction ray from the player's view.

        [SerializeField] private float interactDistance = 2.5f; // Maximum distance where the player can interact.

        [SerializeField] private LayerMask interactableLayer; // Layers that can be detected by the interaction ray.

        [SerializeField] private KeyCode interactKey = KeyCode.E; // Key used to interact with objects.

        private void Update() // Runs once every frame.
        {
            if (Input.GetKeyDown(interactKey)) // Checks if the interaction key was pressed this frame.
            {
                TryInteract(); // Tries to interact with the object in front of the player.
            }
        }

        private void TryInteract() // Checks what the player is looking at and interacts with it.
        {
            if (playerCamera == null) // Stops the method if no camera is assigned.
            {
                Debug.LogWarning("PlayerInteraction: Player Camera is not assigned."); // Shows a warning in the Console.
                return; // Stops the method to avoid errors.
            }

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // Creates a ray from the camera forward.

            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer, QueryTriggerInteraction.Ignore)) // Checks if the ray hits an interactable object.
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>(); // Searches for an interactable component on the hit object or its parents.

                if (interactable != null) // Checks if an interactable component was found.
                {
                    interactable.Interact(); // Runs the interaction logic on the found object.
                }
            }
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
