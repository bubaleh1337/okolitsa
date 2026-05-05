using UnityEngine; 

public class FogZoneController : MonoBehaviour // Controls fog by checking if the player is inside the building zone.
{
    [SerializeField] private Transform player; // Player position used for inside or outside check.

    [SerializeField] private BoxCollider indoorZone; // Building volume where fog must be disabled.

    [SerializeField] private Color fogColor = new Color(0.02f, 0.025f, 0.04f, 1f); // Outdoor fog color.

    [SerializeField] private FogMode fogMode = FogMode.ExponentialSquared; // Fog mode used by Unity.

    [SerializeField] private float indoorFogDensity = 0f; // Fog density inside the building.

    [SerializeField] private float outdoorFogDensity = 0.025f; // Fog density outside the building.

    [SerializeField] private float fadeSpeed = 0.8f; // Speed of fog density transition.

    private void Awake() // Runs once when the object is loaded.
    {
        RenderSettings.fog = true; // Enables Unity fog.

        RenderSettings.fogMode = fogMode; // Applies the selected fog mode.

        RenderSettings.fogColor = fogColor; // Applies the selected fog color.
    }

    private void Update() // Runs once every frame.
    {
        if (player == null || indoorZone == null) // Checks if required references are missing.
        {
            return; // Stops the script safely.
        }

        bool isInside = indoorZone.bounds.Contains(player.position); // Checks if the player is inside the building zone.

        float targetDensity = isInside ? indoorFogDensity : outdoorFogDensity; // Chooses indoor or outdoor fog density.

        RenderSettings.fogDensity = Mathf.MoveTowards(RenderSettings.fogDensity, targetDensity, fadeSpeed * Time.deltaTime); // Smoothly changes fog density.
    }
}