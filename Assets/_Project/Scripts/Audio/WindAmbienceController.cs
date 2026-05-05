using UnityEngine; 

public class WindAmbienceController : MonoBehaviour // Controls indoor and outdoor wind volume.
{
    [SerializeField] private AudioSource indoorWindSource; // Muffled wind heard inside the building.

    [SerializeField] private AudioSource outdoorWindSource; // Natural wind heard outside.

    [SerializeField] private float indoorVolume = 0.015f; // Target indoor wind volume.

    [SerializeField] private float outdoorVolume = 1f; // Target outdoor wind volume.

    [SerializeField] private float fadeSpeed = 2f; // Speed of volume fade.

    [SerializeField] private bool startInside = true; // Defines if the player starts inside.

    private bool isInside; // Stores whether the player is inside.

    private void Awake() // Runs once when the object is loaded.
    {
        isInside = startInside; // Sets the starting location state.
    }

    private void Update() // Runs once every frame.
    {
        float targetIndoorVolume = isInside ? indoorVolume : 0f; // Indoor wind is heard only inside.

        float targetOutdoorVolume = isInside ? 0f : outdoorVolume; // Outdoor wind is heard only outside.

        if (indoorWindSource != null) // Checks if indoor source exists.
        {
            indoorWindSource.volume = Mathf.MoveTowards(indoorWindSource.volume, targetIndoorVolume, fadeSpeed * Time.deltaTime); // Smoothly changes indoor wind volume.
        }

        if (outdoorWindSource != null) // Checks if outdoor source exists.
        {
            outdoorWindSource.volume = Mathf.MoveTowards(outdoorWindSource.volume, targetOutdoorVolume, fadeSpeed * Time.deltaTime); // Smoothly changes outdoor wind volume.
        }
    }

    public void SetInsideState(bool inside) // Changes indoor or outdoor state.
    {
        isInside = inside; // Saves the current location state.
    }
}