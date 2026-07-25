using System; // Gives access to the Action delegate used for power-state notifications.
using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Lighting // Keeps apartment-lighting systems inside a dedicated namespace.
{
    public sealed class ApartmentPowerSupply : MonoBehaviour // Represents electrical power reaching the complete apartment.
    {
        [Header("Power State")] // Groups apartment power settings.
        [SerializeField] private bool powerAvailable = true; // Defines whether electricity currently reaches the apartment.

        [Header("Development Input")] // Groups temporary testing input.
        [SerializeField] private bool enableDebugHotkey = true; // Enables the temporary power-testing key.
        [SerializeField] private KeyCode togglePowerKey = KeyCode.L; // Defines the key used to test the future electrical breaker.

        [Header("Debug")] // Groups development logging.
        [SerializeField] private bool logStateChanges = true; // Defines whether power changes are written to the Console.

        public bool IsPowerAvailable => powerAvailable; // Exposes the current power state as read-only information.

        public event Action<bool> PowerAvailabilityChanged; // Notifies room circuits whenever apartment power changes.

        private void Update() // Runs once per rendered frame.
        {
            if (!enableDebugHotkey) // Checks whether temporary power input is disabled.
            {
                return; // Stops debug-input processing.
            }

            if (Input.GetKeyDown(togglePowerKey)) // Checks whether the configured testing key was pressed.
            {
                TogglePower(); // Simulates changing the future building breaker state.
            }
        }

        public void SetPowerAvailable(bool isAvailable) // Sets the apartment power state explicitly.
        {
            if (powerAvailable == isAvailable) // Checks whether the requested state is already active.
            {
                return; // Prevents unnecessary updates.
            }

            powerAvailable = isAvailable; // Stores the requested power state.

            PowerAvailabilityChanged?.Invoke(powerAvailable); // Notifies all subscribed room circuits.

            if (logStateChanges) // Checks whether development logging is enabled.
            {
                string readableState = powerAvailable ? "Available" : "Unavailable"; // Creates readable state text.

                Debug.Log($"Apartment power changed to: {readableState}.", this); // Reports the new apartment power state.
            }
        }

        public void EnablePower() // Provides a parameterless method for a future breaker or Timeline signal.
        {
            SetPowerAvailable(true); // Restores electricity.
        }

        public void DisablePower() // Provides a parameterless method for a future breaker or Timeline signal.
        {
            SetPowerAvailable(false); // Cuts electricity.
        }

        public void TogglePower() // Provides a parameterless method for testing or breaker interaction.
        {
            SetPowerAvailable(!powerAvailable); // Applies the opposite power state.
        }

#if UNITY_EDITOR // Includes the following testing commands only in the Unity Editor.
        [ContextMenu("Debug/Enable Apartment Power")] // Adds an Inspector command for restoring power.
        private void DebugEnablePower() // Supports manual power testing.
        {
            EnablePower(); // Restores apartment power.
        }

        [ContextMenu("Debug/Disable Apartment Power")] // Adds an Inspector command for cutting power.
        private void DebugDisablePower() // Supports manual power testing.
        {
            DisablePower(); // Cuts apartment power.
        }

        [ContextMenu("Debug/Toggle Apartment Power")] // Adds an Inspector command for toggling power.
        private void DebugTogglePower() // Supports manual power testing.
        {
            TogglePower(); // Changes the current apartment power state.
        }
#endif // Ends the Unity Editor-only section.
    }
}