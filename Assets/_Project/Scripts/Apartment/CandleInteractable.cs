using UnityEngine; // Gives access to Unity engine classes.

public class CandleInteractable : MonoBehaviour, IInteractable // Makes the candle usable through PlayerInteraction.
{
    [Header("Candle State")] // Groups candle state settings in the Inspector.
    [SerializeField] private bool startLit; // Defines whether the candle starts already lit.

    [Header("Candle Visuals")] // Groups visual references in the Inspector.
    [SerializeField] private GameObject flameVisual; // Visible flame object that appears when the candle is lit.
    [SerializeField] private Light candleLight; // Point Light that creates the candle light.

    [Header("Audio")] // Groups audio references in the Inspector.
    [SerializeField] private AudioSource audioSource; // AudioSource used to play the candle lighting sound.
    [SerializeField] private AudioClip lightClip; // Sound played once when the candle is lit.

    private bool isLit; // Stores whether the candle is currently lit.

    public bool IsLit => isLit; // Allows other scripts to check whether the candle is lit.

    private void Awake() // Runs once when the object is loaded.
    {
        isLit = startLit; // Applies the starting candle state.

        if (audioSource == null) // Checks if AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Tries to find AudioSource on the same object.
        }

        ApplyState(); // Applies the correct visual and light state.
    }

    public void Interact() // Runs when the player presses E while looking at the candle.
    {
        if (isLit) // Checks if the candle is already lit.
        {
            return; // Stops the method so the candle cannot be lit twice.
        }

        LightCandle(); // Lights the candle.
    }

    public void LightCandle() // Turns the candle on.
    {
        isLit = true; // Stores that the candle is now lit.

        ApplyState(); // Updates flame and light visuals.

        PlayLightSound(); // Plays the candle lighting sound.
    }

    private void ApplyState() // Applies the current candle state to visuals and light.
    {
        if (flameVisual != null) // Checks if the flame visual is assigned.
        {
            flameVisual.SetActive(isLit); // Shows or hides the flame visual.
        }

        if (candleLight != null) // Checks if the candle light is assigned.
        {
            candleLight.enabled = isLit; // Enables or disables the candle light.
        }
    }

    private void PlayLightSound() // Plays the candle lighting sound if available.
    {
        if (audioSource == null) // Checks if there is no AudioSource.
        {
            return; // Stops safely if audio cannot be played.
        }

        if (lightClip == null) // Checks if there is no assigned lighting sound.
        {
            return; // Stops safely if no clip exists yet.
        }

        audioSource.PlayOneShot(lightClip); // Plays the candle lighting sound once.
    }
}