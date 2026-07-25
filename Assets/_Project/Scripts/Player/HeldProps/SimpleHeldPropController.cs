using System; // Gives access to the Action delegate used for pickup-state notifications.
using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Player // Keeps held-prop functionality inside the OKOLITSA player namespace.
{
    public sealed class SimpleHeldPropController : MonoBehaviour // Stores and displays the one narrative prop currently supported by this sequence.
    {
        [Header("Shoehorn Visual")] // Groups the held shoehorn visual reference.
        [SerializeField] private GameObject heldShoehornVisual; // Stores the shoehorn model displayed beneath the Player camera.

        [Header("Initial State")] // Groups the authored starting state.
        [SerializeField] private bool startWithShoehorn = false; // Defines whether the Player begins the scene already holding the shoehorn.

        [Header("Debug")] // Groups development logging.
        [SerializeField] private bool logStateChanges = true; // Defines whether held-prop state changes are written to the Console.

        [Header("Runtime State")] // Shows the current held-prop state during Play Mode.
        [SerializeField] private bool hasShoehorn; // Stores whether the Player currently possesses the shoehorn.

        public bool HasShoehorn => hasShoehorn; // Exposes the shoehorn possession state as read-only information.

        public event Action<bool> ShoehornStateChanged; // Notifies future narrative systems whenever possession changes.

        private void Awake() // Runs when the Player object is initialized.
        {
            hasShoehorn = startWithShoehorn; // Copies the authored starting state into runtime state.

            ApplyVisualState(); // Synchronizes the held visual with the starting possession state.
        }

        public bool TryAcquireShoehorn() // Attempts to give the shoehorn to the Player.
        {
            if (hasShoehorn) // Checks whether the Player already possesses the shoehorn.
            {
                return false; // Prevents duplicate acquisition.
            }

            hasShoehorn = true; // Stores that the shoehorn has been acquired.

            ApplyVisualState(); // Displays the held shoehorn model.

            ShoehornStateChanged?.Invoke(hasShoehorn); // Notifies future narrative listeners.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                Debug.Log("Player acquired the shoehorn.", this); // Reports successful acquisition.
            }

            return true; // Confirms that the acquisition succeeded.
        }

        public void RemoveShoehorn() // Removes the shoehorn from the Player when a future sequence requires it.
        {
            if (!hasShoehorn) // Checks whether the Player already has no shoehorn.
            {
                return; // Prevents unnecessary state changes.
            }

            hasShoehorn = false; // Stores that the shoehorn is no longer held.

            ApplyVisualState(); // Hides the held shoehorn model.

            ShoehornStateChanged?.Invoke(hasShoehorn); // Notifies future narrative listeners.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                Debug.Log("Player no longer holds the shoehorn.", this); // Reports the removal.
            }
        }

        private void ApplyVisualState() // Synchronizes the held model with the current possession state.
        {
            if (heldShoehornVisual == null) // Checks whether the held visual reference is missing.
            {
                Debug.LogWarning("SimpleHeldPropController: Held Shoehorn Visual is not assigned.", this); // Reports the missing reference.
                return; // Stops safely because no visual can be updated.
            }

            heldShoehornVisual.SetActive(hasShoehorn); // Shows or hides the held shoehorn model.
        }

#if UNITY_EDITOR // Includes manual testing commands only inside the Unity Editor.
        [ContextMenu("Debug/Acquire Shoehorn")] // Adds an Inspector command for testing acquisition.
        private void DebugAcquireShoehorn() // Supports held-prop testing without using the world pickup.
        {
            TryAcquireShoehorn(); // Attempts to acquire the shoehorn.
        }

        [ContextMenu("Debug/Remove Shoehorn")] // Adds an Inspector command for testing removal.
        private void DebugRemoveShoehorn() // Supports held-prop testing without restarting the scene.
        {
            RemoveShoehorn(); // Removes the shoehorn.
        }
#endif // Ends the Unity Editor-only section.
    }
}