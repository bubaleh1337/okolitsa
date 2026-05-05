using UnityEngine;

public class LampFlicker : MonoBehaviour // Controls a flickering horror lamp.
{
    [SerializeField] private Light lampLight; // Light that will flicker.

    [SerializeField] private Renderer bulbRenderer; // Optional visible bulb renderer.

    [SerializeField] private float minIntensity = 0.05f; // Lowest light intensity during flicker.

    [SerializeField] private float maxIntensity = 0.8f; // Highest light intensity during flicker.

    [SerializeField] private float minDelay = 0.03f; // Shortest time between flicker changes.

    [SerializeField] private float maxDelay = 0.18f; // Longest time between flicker changes.

    [SerializeField] private float offChance = 0.25f; // Chance that the lamp briefly turns almost off.

    private float nextFlickerTime; // Stores when the next flicker should happen.

    private Material bulbMaterial; // Stores the bulb material instance.

    private void Awake() // Runs once when the object is loaded.
    {
        if (lampLight == null) // Checks if the light was not assigned.
        {
            lampLight = GetComponentInChildren<Light>(); // Finds a Light in this object or children.
        }

        if (bulbRenderer != null) // Checks if the bulb renderer is assigned.
        {
            bulbMaterial = bulbRenderer.material; // Creates and stores a unique material instance.
        }
    }

    private void Update() // Runs once every frame.
    {
        if (lampLight == null) // Stops if there is no light.
        {
            return; // Exits the method.
        }

        if (Time.time >= nextFlickerTime) // Checks if it is time to change the flicker.
        {
            Flicker(); // Changes the lamp brightness.
        }
    }

    private void Flicker() // Applies one flicker change.
    {
        float intensity = Random.Range(minIntensity, maxIntensity); // Picks a random brightness.

        if (Random.value < offChance) // Randomly decides if the lamp should almost turn off.
        {
            intensity = 0f; // Turns the lamp off for a short moment.
        }

        lampLight.intensity = intensity; // Applies the brightness to the light.

        if (bulbMaterial != null) // Checks if the bulb material exists.
        {
            Color emissionColor = new Color(1f, 0.75f, 0.35f) * intensity; // Creates warm emission color.

            bulbMaterial.SetColor("_EmissionColor", emissionColor); // Changes bulb emission brightness.
        }

        nextFlickerTime = Time.time + Random.Range(minDelay, maxDelay); // Sets the next flicker time.
    }
}
