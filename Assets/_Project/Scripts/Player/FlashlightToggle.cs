using UnityEngine;

public class FlashlightToggle : MonoBehaviour // Controls turning the flashlight on and off.
{
    [SerializeField] private Light flashlightLight; // Light component used as the flashlight beam.

    [SerializeField] private KeyCode toggleKey = KeyCode.F; // Key used to toggle the flashlight.

    [SerializeField] private bool startEnabled = true; // Defines whether the flashlight starts turned on.

    [SerializeField] private AudioSource audioSource; // Audio source used to play flashlight sounds.

    [SerializeField] private AudioClip turnOnClip; // Sound played when the flashlight turns on.

    [SerializeField] private AudioClip turnOffClip; // Sound played when the flashlight turns off.

    private void Awake() // Runs once when the object is loaded.
    {
        if (flashlightLight == null) // Checks if the flashlight light was not assigned manually.
        {
            flashlightLight = GetComponentInChildren<Light>(); // Tries to find a Light component on this object or its children.
        }

        if (audioSource == null) // Checks if AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Finds AudioSource on this object.
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

        PlayToggleSound(); // Plays the correct flashlight sound.
    }

    private void PlayToggleSound() // Plays on or off sound.
    {
        if (audioSource == null) // Stops if there is no AudioSource.
        {
            return; // Exits the method.
        }

        AudioClip clipToPlay = flashlightLight.enabled ? turnOnClip : turnOffClip; // Chooses sound based on light state.

        if (clipToPlay != null) // Checks if sound exists.
        {
            audioSource.PlayOneShot(clipToPlay); // Plays the selected sound once.
        }
    }
}