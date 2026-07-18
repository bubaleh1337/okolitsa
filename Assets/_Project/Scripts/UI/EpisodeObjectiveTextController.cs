using System.Collections; // Gives access to IEnumerator and coroutines.
using UnityEngine; // Gives access to Unity engine classes.
using UnityEngine.UI; // Gives access to legacy UI Text.

public class EpisodeObjectiveTextController : MonoBehaviour // Controls small objective text for playable prototype episodes.
{
    [Header("UI Reference")] // Groups UI references in the Inspector.
    [SerializeField] private Text objectiveText; // Text component used to display the current objective.

    [Header("Text Behavior")] // Groups text behavior settings in the Inspector.
    [SerializeField] private bool clearOnAwake = true; // Defines whether the objective text starts empty.

    private Coroutine temporaryMessageRoutine; // Stores the currently running temporary message coroutine.

    private void Awake() // Runs once when the object is loaded.
    {
        if (objectiveText == null) // Checks if the Text reference was not assigned manually.
        {
            objectiveText = GetComponent<Text>(); // Tries to find a Text component on the same object.
        }

        if (clearOnAwake) // Checks if the text should start empty.
        {
            ClearObjective(); // Clears the objective text at scene start.
        }
    }

    public void ShowObjective(string message) // Shows a persistent objective message.
    {
        StopTemporaryMessageRoutine(); // Stops any temporary message before showing a new persistent objective.

        SetText(message); // Applies the message to the UI text.
    }

    public void ShowTemporaryObjective(string message, float duration) // Shows a temporary objective message.
    {
        StopTemporaryMessageRoutine(); // Stops any previous temporary message.

        temporaryMessageRoutine = StartCoroutine(TemporaryMessageRoutine(message, duration)); // Starts a new temporary message.
    }

    public void ClearObjective() // Clears the objective text.
    {
        StopTemporaryMessageRoutine(); // Stops any active temporary message.

        SetText(string.Empty); // Clears the UI text.
    }

    private IEnumerator TemporaryMessageRoutine(string message, float duration) // Handles temporary objective display.
    {
        SetText(message); // Shows the temporary message.

        yield return new WaitForSeconds(duration); // Waits for the configured duration.

        SetText(string.Empty); // Clears the message after the delay.

        temporaryMessageRoutine = null; // Clears the routine reference.
    }

    private void SetText(string message) // Applies text safely to the UI component.
    {
        if (objectiveText == null) // Checks if the Text reference is missing.
        {
            return; // Stops safely if there is no UI text.
        }

        objectiveText.text = message; // Updates the visible objective text.
    }

    private void StopTemporaryMessageRoutine() // Stops the current temporary message routine if needed.
    {
        if (temporaryMessageRoutine == null) // Checks if no temporary message is running.
        {
            return; // Stops safely.
        }

        StopCoroutine(temporaryMessageRoutine); // Stops the running temporary message coroutine.

        temporaryMessageRoutine = null; // Clears the routine reference.
    }
}