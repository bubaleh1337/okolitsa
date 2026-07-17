using System.Collections; // Gives access to IEnumerator and coroutines.
using UnityEngine; // Gives access to Unity engine classes.
using Okolitsa.Apartment; // Gives access to ApartmentLightFailureController.

public class Episode01LightWentOutController : MonoBehaviour // Controls Episode 01: "Light Went Out".
{
    [Header("Episode References")] // Groups required scene references in the Inspector.
    [SerializeField] private ApartmentLightFailureController apartmentLightController; // Controls apartment power state.
    [SerializeField] private CandleInteractable candle; // Candle that the player must light during the episode.
    [SerializeField] private Episode01DisturbanceController firstDisturbanceController; // First apartment disturbance after candle lighting.
    [SerializeField] private Episode01WindowImpactController windowImpactController; // Window or balcony impact after the first disturbance.

    [Header("Episode Timing")] // Groups timing settings in the Inspector.
    [SerializeField] private float powerFailureDelay = 4f; // Delay before the apartment power fails.
    [SerializeField] private float disturbanceDelayAfterCandleLit = 1.5f; // Delay before the first disturbance after candle lighting.
    [SerializeField] private float windowImpactDelayAfterDisturbance = 2f; // Delay before the window impact after the first disturbance.

    [Header("Episode Start")] // Groups start behavior settings in the Inspector.
    [SerializeField] private bool startEpisodeOnPlay = true; // Starts the episode automatically when Play Mode begins.
    [SerializeField] private bool forcePowerOnAtStart = true; // Ensures apartment lights are on before the failure event.

    [Header("Debug State")] // Shows internal episode state in the Inspector.
    [SerializeField] private bool hasPowerFailed; // Stores whether the power failure already happened.
    [SerializeField] private bool hasRegisteredCandleLit; // Stores whether the candle lighting event was already registered.
    [SerializeField] private bool hasTriggeredFirstDisturbance; // Stores whether the first disturbance already happened.
    [SerializeField] private bool hasTriggeredWindowImpact; // Stores whether the window impact already happened.

    private Coroutine episodeRoutine; // Stores the currently running episode coroutine.
    private Coroutine disturbanceRoutine; // Stores the currently running disturbance coroutine.
    private Coroutine windowImpactRoutine; // Stores the currently running window impact coroutine.

    private void Start() // Runs once when the scene starts.
    {
        if (forcePowerOnAtStart) // Checks if the episode should start with apartment lights enabled.
        {
            TurnApartmentPowerOnForStart(); // Turns apartment lights on before the failure event.
        }

        if (startEpisodeOnPlay) // Checks if the episode should begin automatically.
        {
            StartEpisode(); // Starts the episode flow.
        }
    }

    private void Update() // Runs once every frame.
    {
        CheckCandleLitState(); // Checks whether the player has lit the candle.
    }

    public void StartEpisode() // Starts Episode 01 from the beginning.
    {
        StopRunningRoutines(); // Stops any previous routines before restarting the episode.

        hasPowerFailed = false; // Resets power failure state.
        hasRegisteredCandleLit = false; // Resets candle progress state.
        hasTriggeredFirstDisturbance = false; // Resets first disturbance state.
        hasTriggeredWindowImpact = false; // Resets window impact state.

        episodeRoutine = StartCoroutine(EpisodeRoutine()); // Starts the timed episode sequence.

        Debug.Log("Episode 01: Started."); // Logs that the episode has started.
    }

    private IEnumerator EpisodeRoutine() // Handles the timed beginning of the episode.
    {
        yield return new WaitForSeconds(powerFailureDelay); // Waits before turning the apartment lights off.

        TriggerPowerFailure(); // Turns off the apartment lights and advances the episode state.
    }

    private void TriggerPowerFailure() // Handles the power failure event.
    {
        if (hasPowerFailed) // Checks if power has already failed.
        {
            return; // Stops the method to prevent duplicate power failure events.
        }

        if (apartmentLightController == null) // Checks if the apartment light controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Apartment Light Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        apartmentLightController.TurnPowerOff(); // Turns off all apartment lights through the existing light system.

        hasPowerFailed = true; // Stores that the power failure has happened.

        Debug.Log("Episode 01: Power failed."); // Logs the power failure event.
    }

    private void CheckCandleLitState() // Checks whether the candle was lit after the power failure.
    {
        if (!hasPowerFailed) // Checks if the power failure has not happened yet.
        {
            return; // Candle progress should not be registered before the power fails.
        }

        if (hasRegisteredCandleLit) // Checks if the candle event was already registered.
        {
            return; // Stops the method to avoid repeated logs and repeated disturbance starts.
        }

        if (candle == null) // Checks if the candle reference is missing.
        {
            return; // Stops safely if no candle is assigned.
        }

        if (!candle.IsLit) // Checks if the candle is still unlit.
        {
            return; // Waits until the player lights the candle.
        }

        RegisterCandleLit(); // Registers that the player restored temporary light.
    }

    private void RegisterCandleLit() // Handles the moment when the player lights the candle.
    {
        hasRegisteredCandleLit = true; // Stores that the candle objective is complete.

        Debug.Log("Episode 01: Candle lit. Temporary light restored."); // Logs the completed candle step.

        disturbanceRoutine = StartCoroutine(FirstDisturbanceRoutine()); // Starts the delayed disturbance event.
    }

    private IEnumerator FirstDisturbanceRoutine() // Waits and then triggers the first apartment disturbance.
    {
        yield return new WaitForSeconds(disturbanceDelayAfterCandleLit); // Waits after the candle is lit.

        TriggerFirstDisturbance(); // Triggers the first apartment disturbance.
    }

    private void TriggerFirstDisturbance() // Starts the first disturbance event.
    {
        if (hasTriggeredFirstDisturbance) // Checks if the first disturbance already happened.
        {
            return; // Stops the method to prevent duplicates.
        }

        if (firstDisturbanceController == null) // Checks if the disturbance controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: First Disturbance Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely if no disturbance controller exists.
        }

        hasTriggeredFirstDisturbance = true; // Stores that the first disturbance has happened.

        firstDisturbanceController.PlayDisturbance(); // Runs the first apartment disturbance.

        Debug.Log("Episode 01: First disturbance completed."); // Logs that the first disturbance was triggered.

        windowImpactRoutine = StartCoroutine(WindowImpactRoutine()); // Starts the delayed window impact event.
    }

    private IEnumerator WindowImpactRoutine() // Waits and then triggers the window impact event.
    {
        yield return new WaitForSeconds(windowImpactDelayAfterDisturbance); // Waits after the first disturbance.

        TriggerWindowImpact(); // Triggers the window or balcony impact.
    }

    private void TriggerWindowImpact() // Starts the window or balcony impact event.
    {
        if (hasTriggeredWindowImpact) // Checks if the window impact already happened.
        {
            return; // Stops the method to prevent duplicates.
        }

        if (windowImpactController == null) // Checks if the window impact controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Window Impact Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely if no window impact controller exists.
        }

        hasTriggeredWindowImpact = true; // Stores that the window impact has happened.

        windowImpactController.PlayImpact(); // Runs the window impact event.

        Debug.Log("Episode 01: Window impact completed."); // Logs that the window impact was triggered.
    }

    private void TurnApartmentPowerOnForStart() // Ensures the apartment starts with power enabled.
    {
        if (apartmentLightController == null) // Checks if the apartment light controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Cannot force power on because Apartment Light Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        apartmentLightController.TurnPowerOn(); // Turns apartment lights on before the episode starts.
    }

    private void StopRunningRoutines() // Stops all running episode routines before restarting the episode.
    {
        if (episodeRoutine != null) // Checks if the episode routine is running.
        {
            StopCoroutine(episodeRoutine); // Stops the episode routine.
        }

        if (disturbanceRoutine != null) // Checks if the disturbance routine is running.
        {
            StopCoroutine(disturbanceRoutine); // Stops the disturbance routine.
        }

        if (windowImpactRoutine != null) // Checks if the window impact routine is running.
        {
            StopCoroutine(windowImpactRoutine); // Stops the window impact routine.
        }

        episodeRoutine = null; // Clears the episode routine reference.
        disturbanceRoutine = null; // Clears the disturbance routine reference.
        windowImpactRoutine = null; // Clears the window impact routine reference.
    }
}