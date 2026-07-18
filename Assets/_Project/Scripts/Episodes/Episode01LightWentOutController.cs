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
    [SerializeField] private float powerFailureDelay = 7f; // Delay before the apartment power fails.
    [SerializeField] private float disturbanceDelayAfterCandleLit = 5f; // Delay before the first disturbance after candle lighting.
    [SerializeField] private float windowImpactDelayAfterDisturbance = 6f; // Delay before the window impact after the first disturbance.
    [SerializeField] private float endingDelayAfterWindowImpact = 4f; // Delay before the ending beat after the window impact.

    [Header("Candle Blowout")] // Groups candle blowout settings in the Inspector.
    [SerializeField] private bool extinguishCandleOnWindowImpact = true; // Defines whether the candle goes out after the window impact.
    [SerializeField] private float candleExtinguishDelayAfterImpact = 0.15f; // Delay before the candle is blown out after the impact.

    [Header("Ending Power Return")] // Groups ending power return settings in the Inspector.
    [SerializeField] private bool returnPowerAtEnd = true; // Defines whether apartment power returns at the end of the episode.
    [SerializeField] private int endingPowerFlickerCount = 3; // Defines how many times the lights flicker before returning.
    [SerializeField] private float endingPowerFlickerInterval = 0.35f; // Defines how fast the final light flicker happens.

    [Header("Episode Start")] // Groups start behavior settings in the Inspector.
    [SerializeField] private bool startEpisodeOnPlay = true; // Starts the episode automatically when Play Mode begins.
    [SerializeField] private bool forcePowerOnAtStart = true; // Ensures apartment lights are on before the failure event.

    [Header("Debug State")] // Shows internal episode state in the Inspector.
    [SerializeField] private bool hasPowerFailed; // Stores whether the power failure already happened.
    [SerializeField] private bool hasRegisteredCandleLit; // Stores whether the candle lighting event was already registered.
    [SerializeField] private bool hasTriggeredFirstDisturbance; // Stores whether the first disturbance already happened.
    [SerializeField] private bool hasTriggeredWindowImpact; // Stores whether the window impact already happened.
    [SerializeField] private bool hasExtinguishedCandle; // Stores whether the candle was blown out.
    [SerializeField] private bool hasCompletedEpisode; // Stores whether the episode ending beat has completed.

    private Coroutine episodeRoutine; // Stores the currently running episode coroutine.
    private Coroutine disturbanceRoutine; // Stores the currently running disturbance coroutine.
    private Coroutine windowImpactRoutine; // Stores the currently running window impact coroutine.
    private Coroutine candleExtinguishRoutine; // Stores the currently running candle extinguish coroutine.
    private Coroutine endingRoutine; // Stores the currently running ending coroutine.

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
        hasExtinguishedCandle = false; // Resets candle extinguish state.
        hasCompletedEpisode = false; // Resets episode completion state.

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

        candleExtinguishRoutine = StartCoroutine(CandleExtinguishRoutine()); // Starts delayed candle blowout.

        endingRoutine = StartCoroutine(EpisodeEndingRoutine()); // Starts the final ending beat.
    }

    private IEnumerator CandleExtinguishRoutine() // Handles candle blowout after the window impact.
    {
        yield return new WaitForSeconds(candleExtinguishDelayAfterImpact); // Waits briefly after the impact.

        ExtinguishCandleAfterImpact(); // Blows out the candle.
    }

    private void ExtinguishCandleAfterImpact() // Extinguishes the candle because of the impact/wind.
    {
        if (hasExtinguishedCandle) // Checks if the candle was already extinguished by this event.
        {
            return; // Stops the method to prevent duplicates.
        }

        if (!extinguishCandleOnWindowImpact) // Checks if candle blowout is disabled.
        {
            return; // Stops safely if the designer disabled this behavior.
        }

        if (candle == null) // Checks if the candle reference is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Cannot extinguish candle because Candle is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        candle.ExtinguishCandle(); // Turns off the candle flame and candle light.

        hasExtinguishedCandle = true; // Stores that the candle was extinguished.

        Debug.Log("Episode 01: Candle was blown out by the window impact."); // Logs the candle blowout beat.
    }

    private IEnumerator EpisodeEndingRoutine() // Handles the final beat of Episode 01.
    {
        yield return new WaitForSeconds(endingDelayAfterWindowImpact); // Waits after the window impact.

        if (returnPowerAtEnd) // Checks if the apartment power should return.
        {
            yield return StartCoroutine(ReturnPowerWithFlickerRoutine()); // Returns power with a short flicker sequence.
        }

        CompleteEpisode(); // Marks the episode as completed.
    }

    private IEnumerator ReturnPowerWithFlickerRoutine() // Returns apartment power with a short unstable flicker.
    {
        if (apartmentLightController == null) // Checks if the apartment light controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Cannot return power because Apartment Light Controller is not assigned."); // Shows a warning in Console.
            yield break; // Stops the coroutine safely.
        }

        int safeFlickerCount = Mathf.Max(0, endingPowerFlickerCount); // Prevents negative flicker count values.

        apartmentLightController.StartFlickerSound(); // Starts controlled flicker audio before the visual flicker begins.

        for (int i = 0; i < safeFlickerCount; i++) // Repeats the configured number of final flickers.
        {
            apartmentLightController.TurnPowerOn(false); // Briefly turns apartment lights on without power-on sound.

            yield return new WaitForSeconds(endingPowerFlickerInterval); // Waits before turning lights off again.

            apartmentLightController.TurnPowerOff(false); // Briefly turns apartment lights off without power-off sound.

            yield return new WaitForSeconds(endingPowerFlickerInterval); // Waits before the next flicker.
        }

        apartmentLightController.StopFlickerSound(); // Stops flicker audio exactly before stable power returns.

        apartmentLightController.TurnPowerOn(true); // Leaves apartment power on and plays the power-on sound.

        Debug.Log("Episode 01: Power returned."); // Logs that the apartment power returned.
    }

    private void CompleteEpisode() // Marks the episode as completed.
    {
        if (hasCompletedEpisode) // Checks if the episode is already complete.
        {
            return; // Prevents duplicate completion.
        }

        hasCompletedEpisode = true; // Stores that the episode has completed.

        Debug.Log("Episode 01: Completed. Power returned, but the apartment is not safe."); // Logs the final episode state.
    }

    private void TurnApartmentPowerOnForStart() // Ensures the apartment starts with power enabled.
    {
        if (apartmentLightController == null) // Checks if the apartment light controller is missing.
        {
            Debug.LogWarning("Episode01LightWentOutController: Cannot force power on because Apartment Light Controller is not assigned."); // Shows a warning in Console.
            return; // Stops safely to avoid errors.
        }

        apartmentLightController.TurnPowerOn(false); // Turns apartment lights on before the episode starts without playing sound.
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

        if (candleExtinguishRoutine != null) // Checks if the candle extinguish routine is running.
        {
            StopCoroutine(candleExtinguishRoutine); // Stops the candle extinguish routine.
        }

        if (endingRoutine != null) // Checks if the ending routine is running.
        {
            StopCoroutine(endingRoutine); // Stops the ending routine.
        }

        episodeRoutine = null; // Clears the episode routine reference.
        disturbanceRoutine = null; // Clears the disturbance routine reference.
        windowImpactRoutine = null; // Clears the window impact routine reference.
        candleExtinguishRoutine = null; // Clears the candle extinguish routine reference.
        endingRoutine = null; // Clears the ending routine reference.
    }
}