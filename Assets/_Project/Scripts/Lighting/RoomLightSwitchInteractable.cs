using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Lighting // Keeps apartment-lighting systems inside a dedicated namespace.
{
    public sealed class RoomLightSwitchInteractable : MonoBehaviour, global::IInteractable // Connects an interactable wall switch to one room circuit.
    {
        [Header("Target Circuit")] // Groups the room-circuit reference.
        [SerializeField] private RoomLightCircuit targetCircuit; // Stores the room circuit controlled by this wall switch.

        [Header("Audio")] // Groups optional mechanical switch sounds.
        [SerializeField] private AudioSource audioSource; // Plays the physical switch-click sound.
        [SerializeField] private AudioClip switchOnClip; // Plays when the wall switch moves into the on position.
        [SerializeField] private AudioClip switchOffClip; // Plays when the wall switch moves into the off position.

        public void Interact() // Runs when PlayerInteraction activates this wall switch.
        {
            if (targetCircuit == null) // Checks whether the switch has a room circuit assigned.
            {
                Debug.LogWarning($"{name}: RoomLightCircuit is not assigned.", this); // Reports the missing required reference.
                return; // Stops safely because there is no circuit to control.
            }

            targetCircuit.ToggleRequestedState(); // Moves the wall switch into its opposite logical position.

            PlaySwitchSound(targetCircuit.IsRequestedOn); // Plays the sound matching the new mechanical switch position.
        }

        private void PlaySwitchSound(bool switchedOn) // Plays the appropriate optional wall-switch sound.
        {
            if (audioSource == null) // Checks whether no AudioSource was assigned.
            {
                return; // Allows the switch to function without audio.
            }

            AudioClip selectedClip = switchedOn ? switchOnClip : switchOffClip; // Selects the sound matching the new switch position.

            if (selectedClip == null) // Checks whether the required sound clip is missing.
            {
                return; // Stops without producing an error.
            }

            audioSource.PlayOneShot(selectedClip); // Plays the selected switch sound once.
        }
    }
}