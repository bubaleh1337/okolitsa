using UnityEngine; // Gives access to Unity engine classes.
using UnityEngine.Events; // Gives access to configurable Unity Events.

namespace Okolitsa.Narrative.FirstNight.FN04 // Keeps the smoking-set behaviour inside the FN-04 narrative namespace.
{
    [DisallowMultipleComponent] // Prevents duplicate pickup controllers on the same GameObject.
    public sealed class FN04SmokingSetPickup : MonoBehaviour, IInteractable // Allows the Player to collect the cigarette pack and lighter as one narrative prop set.
    {
        [Header("Smoking Set References")] // Groups the visible object and interaction Collider references.
        [SerializeField] private GameObject visualsRoot; // Contains the cigarette-pack and lighter visual instances.
        [SerializeField] private Collider interactionCollider; // Receives the Player's interaction ray before the set is collected.

        [Header("Optional Audio")] // Groups optional pickup-audio settings.
        [SerializeField] private AudioSource audioSource; // Plays an optional pickup sound without disabling the controller object.
        [SerializeField] private AudioClip pickupClip; // Contains the optional sound played when the smoking set is collected.

        [Header("Starting State")] // Groups authored scene-start settings.
        [SerializeField] private bool startsCollected; // Defines whether the smoking set begins already collected.

        [Header("Events")] // Groups future narrative connections.
        [SerializeField] private UnityEvent onCollected; // Invokes once after the smoking set is successfully collected.

        [Header("Debug")] // Groups development diagnostics.
        [SerializeField] private bool logStateChanges = true; // Defines whether collection state changes are written to the Console.

        [Header("Runtime State")] // Displays the current collection state during Play Mode.
        [SerializeField] private bool hasBeenCollected; // Stores whether Andrey currently possesses the cigarettes and lighter.

        public bool HasBeenCollected => hasBeenCollected; // Exposes the collection state as read-only information for future balcony logic.

        private void Awake() // Runs when the smoking-set object is initialized.
        {
            if (interactionCollider == null) // Checks whether the interaction Collider was not assigned manually.
            {
                interactionCollider = GetComponent<Collider>(); // Attempts to use a Collider attached to the same GameObject.
            }

            if (audioSource == null) // Checks whether an AudioSource was not assigned manually.
            {
                audioSource = GetComponent<AudioSource>(); // Attempts to use an AudioSource attached to the same GameObject.
            }

            hasBeenCollected = startsCollected; // Applies the authored starting collection state.

            ApplyCollectionState(); // Synchronizes the visuals and Collider with the runtime state.
        }

        public void Interact() // Runs when the Player presses the interaction key while targeting the smoking set.
        {
            CollectSmokingSet(); // Attempts to collect both objects as one narrative set.
        }

        public void CollectSmokingSet() // Provides a reusable public method for normal interaction or future narrative staging.
        {
            if (hasBeenCollected) // Checks whether the set has already been collected.
            {
                return; // Prevents repeated collection and repeated event invocation.
            }

            hasBeenCollected = true; // Records that Andrey now possesses the cigarettes and lighter.

            PlayPickupSound(); // Plays the optional collection sound before hiding the visuals.

            ApplyCollectionState(); // Hides the physical objects and disables further interaction.

            onCollected?.Invoke(); // Notifies future narrative systems that the smoking set was collected.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                Debug.Log("FN-04 smoking set collected.", this); // Reports the successful collection.
            }
        }

        private void ApplyCollectionState() // Synchronizes the scene representation with the stored collection state.
        {
            if (visualsRoot != null) // Checks whether the visible smoking-set root is assigned.
            {
                visualsRoot.SetActive(!hasBeenCollected); // Shows the objects before collection and hides them afterwards.
            }
            else // Runs when the required visual reference is missing.
            {
                Debug.LogWarning($"{name}: Visuals Root is not assigned.", this); // Reports the missing visual reference.
            }

            if (interactionCollider != null) // Checks whether the interaction Collider is available.
            {
                interactionCollider.enabled = !hasBeenCollected; // Disables interaction after successful collection.
            }
            else // Runs when the required Collider reference is missing.
            {
                Debug.LogWarning($"{name}: Interaction Collider is not assigned.", this); // Reports the missing Collider reference.
            }
        }

        private void PlayPickupSound() // Plays the optional pickup sound.
        {
            if (audioSource == null) // Checks whether no AudioSource is available.
            {
                return; // Allows silent collection without producing an error.
            }

            if (pickupClip == null) // Checks whether no pickup clip has been assigned.
            {
                return; // Allows silent collection without producing an error.
            }

            audioSource.PlayOneShot(pickupClip); // Plays the pickup clip once.
        }

#if UNITY_EDITOR // Includes manual testing tools only inside the Unity Editor.
        [ContextMenu("Debug/Collect Smoking Set")] // Adds a manual collection command to the component menu.
        private void DebugCollectSmokingSet() // Supports testing without aiming at the bedside table.
        {
            CollectSmokingSet(); // Collects the set through the normal public method.
        }

        [ContextMenu("Debug/Reset Smoking Set")] // Adds a manual reset command to the component menu.
        private void DebugResetSmokingSet() // Supports repeated scene testing during Play Mode.
        {
            hasBeenCollected = false; // Restores the uncollected state.

            ApplyCollectionState(); // Restores the visuals and interaction Collider.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                Debug.Log("FN-04 smoking set reset.", this); // Reports the manual reset.
            }
        }
#endif // Ends the Unity Editor-only section.
    }
}