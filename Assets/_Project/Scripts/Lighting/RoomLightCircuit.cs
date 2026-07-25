using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Lighting // Keeps apartment-lighting systems inside a dedicated namespace.
{
    public sealed class RoomLightCircuit : MonoBehaviour // Controls one room's lamps and bulb visuals.
    {
        [Header("Power Supply")] // Groups the shared apartment-power reference.
        [SerializeField] private ApartmentPowerSupply powerSupply; // Provides apartment-wide electrical availability.

        [Header("Controlled Lights")] // Groups Light components belonging only to this room.
        [SerializeField] private Light[] controlledLights; // Stores the actual Unity Light components.

        [Header("Bulb Visuals")] // Groups visible bulb meshes and their materials.
        [SerializeField] private Renderer[] bulbRenderers; // Stores the visible bulb renderers belonging to this room.
        [SerializeField] private Material bulbOnMaterial; // Defines the glowing bulb material.
        [SerializeField] private Material bulbOffMaterial; // Defines the unlit bulb material.

        [Header("Initial Switch State")] // Groups the starting wall-switch state.
        [SerializeField] private bool startRequestedOn = false; // Defines whether the wall switch starts in its on position.

        [Header("Debug")] // Groups development logging.
        [SerializeField] private bool logStateChanges = true; // Defines whether circuit changes are written to the Console.

        [Header("Runtime State")] // Shows the current state during Play Mode.
        [SerializeField] private bool requestedOn; // Stores the logical wall-switch position.
        [SerializeField] private bool actuallyOn; // Stores whether the room lamps are currently powered.

        public bool IsRequestedOn => requestedOn; // Exposes the wall-switch position.
        public bool IsActuallyOn => actuallyOn; // Exposes the final powered-light state.

        private void Awake() // Runs when the room circuit is initialized.
        {
            requestedOn = startRequestedOn; // Copies the authored switch state into runtime state.
        }

        private void OnEnable() // Runs whenever the room circuit becomes active.
        {
            if (powerSupply != null) // Checks whether the shared power supply is assigned.
            {
                powerSupply.PowerAvailabilityChanged += HandlePowerAvailabilityChanged; // Subscribes to apartment power changes.
            }
            else // Runs when the required reference is missing.
            {
                Debug.LogWarning($"{name}: ApartmentPowerSupply is not assigned.", this); // Reports the missing power-supply reference.
            }

            ApplyState(); // Applies the correct initial room-light state.
        }

        private void OnDisable() // Runs whenever the room circuit becomes inactive.
        {
            if (powerSupply != null) // Checks whether the circuit was subscribed.
            {
                powerSupply.PowerAvailabilityChanged -= HandlePowerAvailabilityChanged; // Removes the event subscription safely.
            }
        }

        public void ToggleRequestedState() // Changes the wall switch into its opposite position.
        {
            SetRequestedState(!requestedOn); // Applies the opposite requested state.
        }

        public void SetRequestedState(bool shouldBeOn) // Sets the room's wall-switch position explicitly.
        {
            if (requestedOn == shouldBeOn) // Checks whether the requested state is already stored.
            {
                return; // Prevents unnecessary updates.
            }

            requestedOn = shouldBeOn; // Stores the new switch position.

            ApplyState(); // Recalculates the actual light state.

            if (logStateChanges) // Checks whether logging is enabled.
            {
                string switchState = requestedOn ? "On" : "Off"; // Converts the switch state into readable text.
                string lightState = actuallyOn ? "On" : "Off"; // Converts the final lamp state into readable text.

                Debug.Log($"{name}: switch requested {switchState}; lights are {lightState}.", this); // Reports both circuit states.
            }
        }

        public void ApplyState() // Recalculates and applies the final state of this room.
        {
            bool powerIsAvailable = powerSupply != null && powerSupply.IsPowerAvailable; // Reads whether electricity reaches the apartment.

            actuallyOn = requestedOn && powerIsAvailable; // Requires both the wall switch and apartment power.

            ApplyLightComponents(); // Updates the actual Unity Light components.
            ApplyBulbVisuals(); // Updates the visible bulb materials.
        }

        private void ApplyLightComponents() // Applies the calculated state to every assigned Light.
        {
            if (controlledLights == null) // Checks whether the Light array exists.
            {
                return; // Stops safely when no lights are assigned.
            }

            foreach (Light controlledLight in controlledLights) // Iterates through all lights belonging to this room.
            {
                if (controlledLight == null) // Checks whether the current reference is empty.
                {
                    continue; // Skips the missing entry.
                }

                controlledLight.enabled = actuallyOn; // Applies the final circuit state to the Light component.
            }
        }

        private void ApplyBulbVisuals() // Applies the correct glowing or unlit material to visible bulbs.
        {
            if (bulbRenderers == null) // Checks whether the Renderer array exists.
            {
                return; // Stops safely when no bulb visuals are assigned.
            }

            Material selectedMaterial = actuallyOn ? bulbOnMaterial : bulbOffMaterial; // Selects the material matching the powered state.

            if (selectedMaterial == null) // Checks whether the required material is missing.
            {
                return; // Avoids replacing bulb materials with an empty reference.
            }

            foreach (Renderer bulbRenderer in bulbRenderers) // Iterates through every visible bulb belonging to this room.
            {
                if (bulbRenderer == null) // Checks whether the current Renderer reference is empty.
                {
                    continue; // Skips the missing entry.
                }

                bulbRenderer.sharedMaterial = selectedMaterial; // Applies the shared material without creating runtime material copies.
            }
        }

        private void HandlePowerAvailabilityChanged(bool isAvailable) // Responds whenever apartment power changes.
        {
            ApplyState(); // Recalculates the room while preserving its wall-switch position.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                string powerState = isAvailable ? "available" : "unavailable"; // Converts power availability into readable text.
                string lightState = actuallyOn ? "On" : "Off"; // Converts the final lamp state into readable text.

                Debug.Log($"{name}: apartment power is {powerState}; lights are {lightState}.", this); // Reports the resulting room state.
            }
        }

#if UNITY_EDITOR // Includes manual testing commands only inside the Unity Editor.
        [ContextMenu("Debug/Toggle Room Switch")] // Adds a circuit-testing command to the Inspector.
        private void DebugToggleRoomSwitch() // Allows circuit testing without using the Player.
        {
            ToggleRequestedState(); // Changes the logical wall-switch state.
        }

        [ContextMenu("Debug/Apply Current State")] // Adds a state-refresh command to the Inspector.
        private void DebugApplyCurrentState() // Allows assigned references to be tested immediately.
        {
            ApplyState(); // Reapplies Light and bulb states.
        }
#endif // Ends the Unity Editor-only section.
    }
}