using Okolitsa.Player; // Gives access to SimpleHeldPropController.
using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Interaction // Keeps narrative interaction components inside a dedicated namespace.
{
    public sealed class NarrativePickupInteractable : MonoBehaviour, global::IInteractable // Allows one authored world prop to be picked up through the existing PlayerInteraction system.
    {
        [Header("Pickup Target")] // Groups the Player-held-prop reference.
        [SerializeField] private SimpleHeldPropController heldPropController; // Stores the controller that will receive the shoehorn.

        [Header("World Object")] // Groups world-object cleanup settings.
        [SerializeField] private GameObject worldObjectRoot; // Stores the complete world shoehorn object that disappears after pickup.

        [Header("Optional Audio")] // Groups optional pickup-audio settings.
        [SerializeField] private AudioSource pickupAudioSource; // Plays the pickup sound from a persistent external AudioSource.
        [SerializeField] private AudioClip pickupClip; // Stores the sound played when the shoehorn is acquired.

        [Header("Debug")] // Groups development logging.
        [SerializeField] private bool logPickup = true; // Defines whether successful pickup is written to the Console.

        [Header("Runtime State")] // Shows the pickup state during Play Mode.
        [SerializeField] private bool hasBeenPickedUp; // Stores whether this world object has already been collected.

        private Collider[] pickupColliders; // Stores every Collider belonging to the world pickup.

        public bool HasBeenPickedUp => hasBeenPickedUp; // Exposes the pickup state as read-only information.

        private void Awake() // Runs when the world pickup is initialized.
        {
            if (worldObjectRoot == null) // Checks whether the world-object root was not assigned manually.
            {
                worldObjectRoot = gameObject; // Uses the GameObject containing this component as the default root.
            }

            pickupColliders = GetComponentsInChildren<Collider>(true); // Caches all pickup colliders, including inactive children.
        }

        public void Interact() // Runs when PlayerInteraction activates the world shoehorn.
        {
            if (hasBeenPickedUp) // Checks whether the object was already collected.
            {
                return; // Prevents duplicate pickup.
            }

            if (heldPropController == null) // Checks whether the Player-held-prop controller is missing.
            {
                Debug.LogWarning($"{name}: SimpleHeldPropController is not assigned.", this); // Reports the missing required reference.
                return; // Stops safely because the Player cannot receive the item.
            }

            bool acquisitionSucceeded = heldPropController.TryAcquireShoehorn(); // Attempts to give the shoehorn to the Player.

            if (!acquisitionSucceeded) // Checks whether acquisition was rejected.
            {
                return; // Leaves the world object unchanged when the Player already owns the shoehorn.
            }

            hasBeenPickedUp = true; // Stores that the world pickup has been collected.

            DisablePickupColliders(); // Prevents another interaction during cleanup.

            PlayPickupSound(); // Plays the optional pickup sound.

            if (logPickup) // Checks whether development logging is enabled.
            {
                Debug.Log($"{name} was picked up.", this); // Reports successful world-object pickup.
            }

            if (worldObjectRoot != null) // Checks whether the world-object root exists.
            {
                worldObjectRoot.SetActive(false); // Removes the world shoehorn after all pickup work is complete.
            }
        }

        private void DisablePickupColliders() // Disables every collider belonging to the world pickup.
        {
            if (pickupColliders == null) // Checks whether the cached collider array is unavailable.
            {
                return; // Stops safely.
            }

            foreach (Collider pickupCollider in pickupColliders) // Iterates through every cached pickup collider.
            {
                if (pickupCollider == null) // Checks whether the current collider reference is empty.
                {
                    continue; // Skips the missing entry.
                }

                pickupCollider.enabled = false; // Prevents further raycast interaction.
            }
        }

        private void PlayPickupSound() // Plays the optional acquisition sound.
        {
            if (pickupAudioSource == null) // Checks whether no persistent AudioSource was assigned.
            {
                return; // Allows the pickup to work without audio.
            }

            if (pickupClip == null) // Checks whether no pickup clip was assigned.
            {
                return; // Stops without producing an error.
            }

            pickupAudioSource.PlayOneShot(pickupClip); // Plays the pickup sound once.
        }
    }
}