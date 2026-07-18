using UnityEngine; // Gives access to Unity engine classes.
using UnityEngine.UI; // Gives access to legacy UI Text.

public class InteractionPromptController : MonoBehaviour // Controls the small interaction prompt shown near the screen center.
{
    [Header("UI Reference")] // Groups UI references in the Inspector.
    [SerializeField] private Text promptText; // Text component used to display the interaction prompt.

    [Header("Prompt Settings")] // Groups prompt text settings in the Inspector.
    [SerializeField] private string defaultPrompt = "E — Interact"; // Default text shown when the player looks at an interactable object.
    [SerializeField] private bool hideOnAwake = true; // Defines whether the prompt starts hidden.

    private void Awake() // Runs once when the object is loaded.
    {
        if (promptText == null) // Checks if the Text reference was not assigned manually.
        {
            promptText = GetComponent<Text>(); // Tries to find Text on the same object.
        }

        if (hideOnAwake) // Checks if the prompt should start hidden.
        {
            HidePrompt(); // Hides the prompt at scene start.
        }
    }

    public void ShowDefaultPrompt() // Shows the default interaction prompt.
    {
        ShowPrompt(defaultPrompt); // Shows the configured default prompt text.
    }

    public void ShowPrompt(string message) // Shows a custom interaction prompt.
    {
        if (promptText == null) // Checks if the Text reference is missing.
        {
            return; // Stops safely if there is no UI text.
        }

        promptText.text = message; // Updates the visible prompt text.

        promptText.enabled = true; // Makes the prompt visible.
    }

    public void HidePrompt() // Hides the interaction prompt.
    {
        if (promptText == null) // Checks if the Text reference is missing.
        {
            return; // Stops safely if there is no UI text.
        }

        promptText.text = string.Empty; // Clears the prompt text.

        promptText.enabled = false; // Hides the prompt component.
    }
}