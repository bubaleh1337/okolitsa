using System.Collections; // Gives access to IEnumerator and coroutines.
using UnityEngine; // Gives access to Unity engine classes.

public class Episode01WindowImpactController : MonoBehaviour // Controls the window or balcony impact event for Episode 01.
{
    [Header("Audio")] // Groups audio settings in the Inspector.
    [SerializeField] private AudioSource audioSource; // AudioSource used to play the impact sound.
    [SerializeField] private AudioClip impactClip; // Sound played when the impact happens.
    [SerializeField] private float impactVolume = 1f; // Volume multiplier for the impact sound.

    [Header("Optional Visual Shake")] // Groups optional object shake settings in the Inspector.
    [SerializeField] private Transform objectToShake; // Optional object that will shake during the impact.
    [SerializeField] private float shakeDuration = 0.25f; // How long the object shake lasts.
    [SerializeField] private float shakeAmount = 0.035f; // How far the object moves during the shake.

    [Header("Debug State")] // Shows internal state in the Inspector.
    [SerializeField] private bool hasPlayed; // Stores whether the impact already happened.

    private Vector3 originalLocalPosition; // Stores the original local position of the object to shake.
    private Coroutine shakeRoutine; // Stores the currently running shake coroutine.

    private void Awake() // Runs once when the object is loaded.
    {
        if (audioSource == null) // Checks if AudioSource was not assigned manually.
        {
            audioSource = GetComponent<AudioSource>(); // Tries to find AudioSource on this object.
        }

        CacheOriginalShakePosition(); // Stores the original position for safe restoration after shaking.
    }

    public void PlayImpact() // Runs the window or balcony impact event once.
    {
        if (hasPlayed) // Checks if the impact already happened.
        {
            return; // Prevents repeated impact events.
        }

        hasPlayed = true; // Marks the impact as completed.

        PlayImpactSound(); // Plays the impact sound.

        StartObjectShake(); // Starts optional visual shake.

        Debug.Log("Episode 01: Window impact triggered."); // Logs the impact event.
    }

    public void ResetImpact() // Resets the impact for testing in Play Mode.
    {
        hasPlayed = false; // Allows the impact to be triggered again.

        StopObjectShake(); // Stops shake if it is still running.

        RestoreOriginalShakePosition(); // Restores the object to its original local position.
    }

    private void PlayImpactSound() // Plays the assigned impact sound if available.
    {
        if (audioSource == null) // Checks if there is no AudioSource.
        {
            Debug.LogWarning("Episode01WindowImpactController: AudioSource is not assigned."); // Shows a warning in Console.
            return; // Stops safely if audio cannot be played.
        }

        if (impactClip == null) // Checks if there is no assigned impact clip.
        {
            Debug.LogWarning("Episode01WindowImpactController: Impact Clip is not assigned."); // Shows a warning in Console.
            return; // Stops safely if no clip exists yet.
        }

        audioSource.PlayOneShot(impactClip, impactVolume); // Plays the impact sound once.
    }

    private void StartObjectShake() // Starts shaking the optional visual object.
    {
        if (objectToShake == null) // Checks if no object is assigned for shaking.
        {
            return; // Stops safely because visual shake is optional.
        }

        StopObjectShake(); // Stops any previous shake routine before starting a new one.

        shakeRoutine = StartCoroutine(ShakeRoutine()); // Starts the shake coroutine.
    }

    private IEnumerator ShakeRoutine() // Moves the object slightly for a short impact effect.
    {
        float elapsedTime = 0f; // Tracks how much time has passed.

        while (elapsedTime < shakeDuration) // Runs while the shake duration has not ended.
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeAmount; // Creates a small random movement offset.
            randomOffset.y = 0f; // Keeps the shake mostly horizontal to avoid strange vertical movement.

            objectToShake.localPosition = originalLocalPosition + randomOffset; // Applies the shake offset.

            elapsedTime += Time.deltaTime; // Increases elapsed time.

            yield return null; // Waits until the next frame.
        }

        RestoreOriginalShakePosition(); // Restores the object after the shake ends.

        shakeRoutine = null; // Clears the routine reference.
    }

    private void StopObjectShake() // Stops the current shake coroutine if needed.
    {
        if (shakeRoutine == null) // Checks if no shake routine is running.
        {
            return; // Stops safely.
        }

        StopCoroutine(shakeRoutine); // Stops the active shake coroutine.

        shakeRoutine = null; // Clears the routine reference.
    }

    private void CacheOriginalShakePosition() // Stores the starting local position of the shake object.
    {
        if (objectToShake == null) // Checks if no object is assigned.
        {
            return; // Stops safely because shake is optional.
        }

        originalLocalPosition = objectToShake.localPosition; // Saves the starting local position.
    }

    private void RestoreOriginalShakePosition() // Restores the shake object to its original local position.
    {
        if (objectToShake == null) // Checks if no object is assigned.
        {
            return; // Stops safely because shake is optional.
        }

        objectToShake.localPosition = originalLocalPosition; // Restores the original local position.
    }
}