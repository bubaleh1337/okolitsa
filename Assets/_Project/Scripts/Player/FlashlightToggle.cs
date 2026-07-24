using UnityEngine; // Gives access to Unity engine classes.

public class FlashlightToggle : MonoBehaviour // Controls flashlight input, visual state, and toggle audio.
{
    [Header("Flashlight")] // Groups flashlight references and initial state.
    [SerializeField] private Light flashlightLight; // Light component used as the flashlight beam.
    [SerializeField] private bool startEnabled = true; // Defines whether the flashlight starts turned on.

    [Header("Input")] // Groups flashlight-input settings.
    [SerializeField] private KeyCode toggleKey = KeyCode.F; // Key used to toggle the flashlight.
    [SerializeField] private bool inputEnabled = true; // Defines whether the player can currently use the flashlight key.

    [Header("Audio")] // Groups flashlight-audio settings.
    [SerializeField] private AudioSource audioSource; // AudioSource used to play flashlight sounds.
    [SerializeField] private AudioClip turnOnClip; // Sound played when the flashlight turns on.
    [SerializeField] private AudioClip turnOffClip; // Sound played when the flashlight turns off.

    public bool IsInputEnabled => inputEnabled; // Exposes whether flashlight input is currently allowed.
    public bool IsFlashlightEnabled => flashlightLight != null && flashlightLight.enabled; // Exposes the current flashlight state.

    private void Awake() // Runs when the object is initialized.
    {
        if (flashlightLight == null) // Checks whether the flashlight Light was not assigned manually.
        {
            flashlightLight = GetComponentInChildren<Light>(); // Tries to find a child Light component.
        }

        if (audioSource == null) // Checks whether AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Tries to find AudioSource on this object.
        }

        if (flashlightLight != null) // Checks whether a flashlight Light was found.
        {
            flashlightLight.enabled = startEnabled; // Applies the configured starting state.
        }
    }

    private void Update() // Runs once per frame.
    {
        if (!inputEnabled) // Checks whether flashlight input is currently blocked.
        {
            return; // Stops input processing.
        }

        if (Input.GetKeyDown(toggleKey)) // Checks whether the flashlight key was pressed.
        {
            ToggleFlashlight(); // Switches the flashlight state.
        }
    }

    private void ToggleFlashlight() // Toggles the current flashlight state.
    {
        if (flashlightLight == null) // Checks whether the flashlight Light is missing.
        {
            Debug.LogWarning("FlashlightToggle: Flashlight Light is not assigned."); // Reports the missing reference.
            return; // Stops safely.
        }

        SetFlashlightState(!flashlightLight.enabled, true); // Applies the opposite flashlight state and plays audio.
    }

    public void SetInputEnabled(bool isEnabled) // Changes whether the player may use flashlight input.
    {
        inputEnabled = isEnabled; // Stores the requested input permission.
    }

    public void SetFlashlightState(bool isEnabled, bool playSound) // Sets the flashlight state explicitly for gameplay or Timeline events.
    {
        if (flashlightLight == null) // Checks whether the flashlight Light is missing.
        {
            Debug.LogWarning("FlashlightToggle: Flashlight Light is not assigned."); // Reports the missing reference.
            return; // Stops safely.
        }

        if (flashlightLight.enabled == isEnabled) // Checks whether the requested state is already active.
        {
            return; // Prevents duplicate state changes and duplicate sounds.
        }

        flashlightLight.enabled = isEnabled; // Applies the requested flashlight state.

        if (playSound) // Checks whether the state change should produce audio.
        {
            PlayToggleSound(); // Plays the correct flashlight sound.
        }
    }

    private void PlayToggleSound() // Plays the sound associated with the current flashlight state.
    {
        if (audioSource == null) // Checks whether AudioSource is missing.
        {
            return; // Stops safely.
        }

        AudioClip clipToPlay = flashlightLight.enabled ? turnOnClip : turnOffClip; // Selects the on or off sound.

        if (clipToPlay != null) // Checks whether the selected sound exists.
        {
            audioSource.PlayOneShot(clipToPlay); // Plays the selected sound once.
        }
    }
}