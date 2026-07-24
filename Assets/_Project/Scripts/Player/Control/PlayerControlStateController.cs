using Irka.Player; // Gives access to the existing SimpleFPSController.
using UnityEngine; // Gives access to Unity engine classes.
using FlashlightToggleComponent = global::FlashlightToggle; // Creates a clear alias for the existing global FlashlightToggle class.
using PlayerInteractionComponent = global::PlayerInteraction; // Creates a clear alias for the existing global PlayerInteraction class.

namespace Okolitsa.Player // Keeps the new reusable control architecture inside the OKOLITSA player namespace.
{
    public sealed class PlayerControlStateController : MonoBehaviour // Coordinates player permissions without depending on a specific narrative sequence.
    {
        [Header("Player Components")] // Groups controlled player components in the Inspector.
        [SerializeField] private SimpleFPSController fpsController; // Controls movement and camera-look permissions.
        [SerializeField] private PlayerInteractionComponent playerInteraction; // Controls interaction detection, prompts, and E input.
        [SerializeField] private FlashlightToggleComponent flashlightToggle; // Controls flashlight input.

        [Header("Initial State")] // Groups starting-mode settings.
        [SerializeField] private PlayerControlMode startingMode = PlayerControlMode.FullControl; // Defines the mode applied when the scene starts.

        [Header("Debug Hotkeys")] // Groups temporary development controls.
        [SerializeField] private bool enableDebugHotkeys = true; // Enables manual mode testing during development.
        [SerializeField] private KeyCode noControlKey = KeyCode.F6; // Switches to NoControl during testing.
        [SerializeField] private KeyCode lookOnlyKey = KeyCode.F7; // Switches to LookOnly during testing.
        [SerializeField] private KeyCode fullControlKey = KeyCode.F8; // Switches to FullControl during testing.
        [SerializeField] private bool logStateChanges = true; // Writes mode changes to the Console.

        [Header("Runtime State")] // Shows the active mode in the Inspector.
        [SerializeField] private PlayerControlMode currentMode; // Stores the currently applied player-control mode.

        public PlayerControlMode CurrentMode => currentMode; // Exposes the current mode as read-only state.

        private void Awake() // Runs when the player object is initialized.
        {
            CacheMissingReferences(); // Finds controlled components that were not assigned manually.

            SetControlMode(startingMode); // Applies the configured starting mode.
        }

        private void Update() // Runs once per frame.
        {
            if (!enableDebugHotkeys) // Checks whether manual testing keys are disabled.
            {
                return; // Stops debug input processing.
            }

            if (Input.GetKeyDown(noControlKey)) // Checks whether the NoControl test key was pressed.
            {
                SetNoControl(); // Applies the fully blocked control mode.
            }

            if (Input.GetKeyDown(lookOnlyKey)) // Checks whether the LookOnly test key was pressed.
            {
                SetLookOnly(); // Applies camera-look-only control.
            }

            if (Input.GetKeyDown(fullControlKey)) // Checks whether the FullControl test key was pressed.
            {
                SetFullControl(); // Restores normal gameplay control.
            }
        }

        public void SetNoControl() // Provides a parameterless method suitable for Timeline signals and UnityEvents.
        {
            SetControlMode(PlayerControlMode.NoControl); // Applies the NoControl mode.
        }

        public void SetLookOnly() // Provides a parameterless method suitable for Timeline signals and UnityEvents.
        {
            SetControlMode(PlayerControlMode.LookOnly); // Applies the LookOnly mode.
        }

        public void SetFullControl() // Provides a parameterless method suitable for Timeline signals and UnityEvents.
        {
            SetControlMode(PlayerControlMode.FullControl); // Applies the FullControl mode.
        }

        public void SetControlMode(PlayerControlMode newMode) // Applies one of the reusable player-control modes.
        {
            currentMode = newMode; // Stores the requested mode.

            bool allowMovement = currentMode == PlayerControlMode.FullControl; // Allows movement only during normal gameplay.
            bool allowLook = currentMode != PlayerControlMode.NoControl; // Allows camera look during LookOnly and FullControl.
            bool allowInteraction = currentMode == PlayerControlMode.FullControl; // Allows interaction only during normal gameplay.
            bool allowFlashlightInput = currentMode == PlayerControlMode.FullControl; // Allows flashlight input only during normal gameplay.

            if (fpsController != null) // Checks whether the FPS controller reference exists.
            {
                fpsController.SetControlPermissions(allowMovement, allowLook); // Applies movement and camera-look permissions.
            }

            if (playerInteraction != null) // Checks whether the interaction controller reference exists.
            {
                playerInteraction.SetInteractionEnabled(allowInteraction); // Applies interaction permission and prompt visibility.
            }

            if (flashlightToggle != null) // Checks whether the flashlight controller reference exists.
            {
                flashlightToggle.SetInputEnabled(allowFlashlightInput); // Applies flashlight-input permission.
            }

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                Debug.Log($"Player control mode changed to: {currentMode}."); // Reports the active mode in the Console.
            }
        }

        private void CacheMissingReferences() // Finds player components that were not assigned in the Inspector.
        {
            if (fpsController == null) // Checks whether the FPS controller reference is missing.
            {
                fpsController = GetComponent<SimpleFPSController>(); // Finds the FPS controller on the Player.
            }

            if (playerInteraction == null) // Checks whether the interaction reference is missing.
            {
                playerInteraction = GetComponent<PlayerInteractionComponent>(); // Finds PlayerInteraction on the Player.
            }

            if (flashlightToggle == null) // Checks whether the flashlight reference is missing.
            {
                flashlightToggle = GetComponentInChildren<FlashlightToggleComponent>(true); // Finds FlashlightToggle on the Player or its children.
            }

            if (fpsController == null) // Checks whether the required FPS controller still could not be found.
            {
                Debug.LogWarning("PlayerControlStateController: SimpleFPSController is not assigned."); // Reports the missing reference.
            }

            if (playerInteraction == null) // Checks whether PlayerInteraction still could not be found.
            {
                Debug.LogWarning("PlayerControlStateController: PlayerInteraction is not assigned."); // Reports the missing reference.
            }

            if (flashlightToggle == null) // Checks whether FlashlightToggle still could not be found.
            {
                Debug.LogWarning("PlayerControlStateController: FlashlightToggle is not assigned."); // Reports the missing reference.
            }
        }
    }
}