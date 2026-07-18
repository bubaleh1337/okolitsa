using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Apartment // Keeps apartment-related systems grouped under the Okolitsa apartment namespace.
{
    public class ApartmentLightFailureController : MonoBehaviour // Controls apartment light power state.
    {
        [Header("Apartment Lights")] // Groups real light references in the Inspector.
        [SerializeField] private Light[] apartmentLights; // Lights that belong to the apartment power system.

        [Header("Light Visuals")] // Groups visible bulb renderer settings in the Inspector.
        [SerializeField] private Renderer[] bulbRenderers; // Visible bulb meshes that should change material with power state.
        [SerializeField] private Material bulbOnMaterial; // Material used when the bulbs are powered.
        [SerializeField] private Material bulbOffMaterial; // Material used when the bulbs are not powered.

        [Header("Audio")] // Groups apartment power audio settings in the Inspector.
        [SerializeField] private AudioSource audioSource; // AudioSource used to play power and flicker sounds.
        [SerializeField] private AudioClip powerOffClip; // Sound played when apartment power turns off.
        [SerializeField] private AudioClip powerOnClip; // Sound played when apartment power turns on.
        [SerializeField] private AudioClip flickerClip; // Sound played during unstable light flicker.
        [SerializeField] private float audioVolume = 1f; // Volume multiplier for apartment power sounds.

        [Header("Debug")] // Groups debug controls in the Inspector.
        [SerializeField] private KeyCode testToggleKey = KeyCode.L; // Debug key used to toggle apartment power.
        [SerializeField] private bool startWithPowerOn = true; // Defines whether apartment lights start enabled.

        private bool isPowerOn; // Stores the current apartment power state.

        public bool IsPowerOn => isPowerOn; // Allows other systems to read the current power state.

        private void Awake() // Runs once when the object is loaded.
        {
            isPowerOn = startWithPowerOn; // Applies the configured starting power state.

            if (audioSource == null) // Checks if AudioSource was not assigned manually.
            {
                audioSource = GetComponent<AudioSource>(); // Tries to find AudioSource on the same object.
            }

            ApplyLightState(); // Applies the starting light state without playing sound.
        }

        private void Update() // Runs once every frame.
        {
            if (Input.GetKeyDown(testToggleKey)) // Checks if the debug toggle key was pressed.
            {
                TogglePower(); // Toggles apartment power for testing.
            }
        }

        public void TurnPowerOff() // Turns apartment power off with sound.
        {
            TurnPowerOff(true); // Uses the overload that allows sound control.
        }

        public void TurnPowerOff(bool playSound) // Turns apartment power off and optionally plays sound.
        {
            SetPowerState(false, playSound); // Applies the off state.
        }

        public void TurnPowerOn() // Turns apartment power on with sound.
        {
            TurnPowerOn(true); // Uses the overload that allows sound control.
        }

        public void TurnPowerOn(bool playSound) // Turns apartment power on and optionally plays sound.
        {
            SetPowerState(true, playSound); // Applies the on state.
        }

        public void TogglePower() // Toggles apartment power with sound.
        {
            SetPowerState(!isPowerOn, true); // Switches to the opposite power state.
        }

        public void PlayFlickerSound() // Plays a dedicated flicker sound without changing light state.
        {
            PlaySound(flickerClip); // Plays the assigned flicker clip.
        }

        private void SetPowerState(bool powerOn, bool playSound) // Applies a power state and optionally plays audio.
        {
            if (isPowerOn == powerOn) // Checks if the requested state is already active.
            {
                ApplyLightState(); // Reapplies visuals in case references changed.
                return; // Stops without replaying audio.
            }

            isPowerOn = powerOn; // Stores the new power state.

            ApplyLightState(); // Applies the new power state to lights and bulbs.

            if (playSound) // Checks if sound should be played.
            {
                PlayPowerStateSound(); // Plays the correct power on or off sound.
            }
        }

        private void ApplyLightState() // Applies the current power state to all apartment light elements.
        {
            ApplyRealLights(); // Enables or disables real Light components.

            ApplyBulbVisuals(); // Changes visible bulb materials.
        }

        private void ApplyRealLights() // Enables or disables apartment Light components.
        {
            if (apartmentLights == null) // Checks if the light array is missing.
            {
                return; // Stops safely.
            }

            foreach (Light apartmentLight in apartmentLights) // Loops through all apartment lights.
            {
                if (apartmentLight == null) // Checks if this light slot is empty.
                {
                    continue; // Skips empty slots safely.
                }

                apartmentLight.enabled = isPowerOn; // Applies the current power state.
            }
        }

        private void ApplyBulbVisuals() // Applies powered or unpowered bulb material.
        {
            if (bulbRenderers == null) // Checks if the renderer array is missing.
            {
                return; // Stops safely.
            }

            Material targetMaterial = isPowerOn ? bulbOnMaterial : bulbOffMaterial; // Selects the material for the current state.

            if (targetMaterial == null) // Checks if the target material is not assigned.
            {
                return; // Stops safely without changing materials.
            }

            foreach (Renderer bulbRenderer in bulbRenderers) // Loops through all bulb renderers.
            {
                if (bulbRenderer == null) // Checks if this renderer slot is empty.
                {
                    continue; // Skips empty slots safely.
                }

                bulbRenderer.sharedMaterial = targetMaterial; // Applies the selected material.
            }
        }

        private void PlayPowerStateSound() // Plays power on or power off sound.
        {
            AudioClip targetClip = isPowerOn ? powerOnClip : powerOffClip; // Chooses sound based on the current power state.

            PlaySound(targetClip); // Plays the selected sound.
        }

        private void PlaySound(AudioClip clip) // Plays one sound if audio references are valid.
        {
            if (audioSource == null) // Checks if there is no AudioSource.
            {
                return; // Stops safely if audio cannot be played.
            }

            if (clip == null) // Checks if no clip is assigned.
            {
                return; // Stops safely if no clip exists yet.
            }

            audioSource.PlayOneShot(clip, audioVolume); // Plays the selected sound once.
        }
    }
}