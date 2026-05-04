using UnityEngine;

public class FlashlightToggle : MonoBehaviour // Controls turning the flashlight on and off.
{
    [SerializeField] private Light flashlightLight; // Light component used as the flashlight beam.

    [SerializeField] private KeyCode toggleKey = KeyCode.F; // Key used to toggle the flashlight.

    [SerializeField] private bool startEnabled = true; // Defines whether the flashlight starts turned on.

    private void Awake() // Runs once when the object is loaded.
    {
        if (flashlightLight == null) // Checks if the flashlight light was not assigned manually.
        {
            flashlightLight = GetComponentInChildren<Light>(); // Tries to find a Light component on this object or its children.
        }

        if (flashlightLight != null) // Checks if a Light component was found.
        {
            flashlightLight.enabled = startEnabled; // Sets the starting flashlight state.
        }
    }

    private void Update() // Runs once every frame.
    {
        if (Input.GetKeyDown(toggleKey)) // Checks if the toggle key was pressed this frame.
        {
            ToggleFlashlight(); // Switches the flashlight on or off.
        }
    }

    private void ToggleFlashlight() // Changes the flashlight state.
    {
        if (flashlightLight == null) // Stops if no flashlight light exists.
        {
            Debug.LogWarning("FlashlightToggle: Flashlight Light is not assigned."); // Shows a warning in the Console.
            return; // Stops the method.
        }

        flashlightLight.enabled = !flashlightLight.enabled; // Turns the flashlight on if it is off, or off if it is on.
    }
}