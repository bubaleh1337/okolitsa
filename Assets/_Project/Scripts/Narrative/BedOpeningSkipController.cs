using System.Collections; // Gives access to IEnumerator for restoring control on the next frame.
using Irka.Player; // Gives access to PlayerLookLimiter.
using Okolitsa.Narrative.CameraSystem; // Gives access to CameraPoseHandoff.
using Okolitsa.Player; // Gives access to PlayerControlStateController.
using UnityEngine; // Gives access to Unity engine classes.
using UnityEngine.Playables; // Gives access to PlayableDirector and PlayState.

namespace Okolitsa.Narrative // Keeps the sequence-specific skip controller inside the narrative namespace.
{
    public sealed class BedOpeningSkipController : MonoBehaviour // Skips the complete bed-opening sequence and restores a safe gameplay state.
    {
        [Header("Timeline Directors")] // Groups Timeline references in the Inspector.
        [SerializeField] private PlayableDirector masterDirector; // Stores the master Timeline controlling the complete opening.
        [SerializeField] private PlayableDirector[] subDirectors; // Stores the FN01 and FN02 Sub-Timeline directors.

        [Header("Camera Handoff")] // Groups camera-related references.
        [SerializeField] private CameraPoseHandoff cameraPoseHandoff; // Stores the active camera-handoff component.
        [SerializeField] private Camera cutsceneCamera; // Stores the shared bed cutscene camera.
        [SerializeField] private Camera gameplayCamera; // Stores the normal Player camera.

        [Header("Player Control")] // Groups Player control-system references.
        [SerializeField] private PlayerControlStateController playerControlStateController; // Restores normal gameplay permissions.
        [SerializeField] private PlayerLookLimiter playerLookLimiter; // Ends the temporary constrained-look state.

        [Header("Sequence Content")] // Groups temporary narrative content that must be cleaned up.
        [SerializeField] private GameObject sleepParalysisFigure; // Stores the temporary sleep-paralysis figure.
        [SerializeField] private AudioSource[] sequenceAudioSources; // Stores sounds that must stop when the sequence is skipped.

        [Header("Skip Input")] // Groups skip-input settings.
        [SerializeField] private bool allowSkip = true; // Defines whether the sequence may currently be skipped.
        [SerializeField] private KeyCode skipKey = KeyCode.Space; // Defines the key used to skip the opening.

        [Header("Debug")] // Groups development logging.
        [SerializeField] private bool logSkip = true; // Defines whether a successful skip is written to the Console.

        private Vector3 gameplayCameraLocalPosition; // Stores the normal local position of the gameplay camera.
        private Quaternion gameplayCameraLocalRotation; // Stores the normal local rotation of the gameplay camera.
        private float gameplayCameraFieldOfView; // Stores the normal gameplay-camera field of view.
        private bool hasSkipped; // Prevents the sequence from being skipped more than once.

        private void Awake() // Runs when the master sequence object is initialized.
        {
            if (gameplayCamera == null) // Checks whether the gameplay camera reference is missing.
            {
                Debug.LogWarning("BedOpeningSkipController: Gameplay Camera is not assigned.", this); // Reports the missing reference.
                return; // Stops pose caching safely.
            }

            gameplayCameraLocalPosition = gameplayCamera.transform.localPosition; // Saves the normal camera position inside the Player.
            gameplayCameraLocalRotation = gameplayCamera.transform.localRotation; // Saves the normal camera rotation inside the Player.
            gameplayCameraFieldOfView = gameplayCamera.fieldOfView; // Saves the normal gameplay field of view.
        }

        private void Update() // Runs once per rendered frame.
        {
            if (!allowSkip) // Checks whether skipping is disabled.
            {
                return; // Stops skip-input processing.
            }

            if (hasSkipped) // Checks whether the opening has already been skipped.
            {
                return; // Prevents duplicate cleanup.
            }

            if (masterDirector == null) // Checks whether the master Timeline reference is missing.
            {
                return; // Stops safely.
            }

            if (masterDirector.state != PlayState.Playing) // Checks whether the opening Timeline is currently running.
            {
                return; // Allows Space to behave normally after the opening ends.
            }

            if (Input.GetKeyDown(skipKey)) // Checks whether the configured skip key was pressed.
            {
                SkipOpening(); // Ends the sequence and restores gameplay.
            }
        }

        public void SkipOpening() // Provides an explicit public skip method.
        {
            if (hasSkipped) // Checks whether cleanup has already happened.
            {
                return; // Prevents repeated state changes.
            }

            hasSkipped = true; // Marks the sequence as skipped.

            if (cameraPoseHandoff != null) // Checks whether a camera blend may currently be running.
            {
                cameraPoseHandoff.StopAllCoroutines(); // Stops any incomplete camera-handoff coroutine.
            }

            if (masterDirector != null) // Checks whether the master Director exists.
            {
                masterDirector.Stop(); // Stops the master Timeline and its controlled content.
            }

            if (subDirectors != null) // Checks whether the Sub-Timeline array exists.
            {
                foreach (PlayableDirector subDirector in subDirectors) // Iterates through every assigned Sub-Timeline Director.
                {
                    if (subDirector == null) // Checks whether the current array entry is empty.
                    {
                        continue; // Skips the missing reference.
                    }

                    subDirector.Stop(); // Stops the Sub-Timeline explicitly.
                }
            }

            if (sequenceAudioSources != null) // Checks whether sequence audio references exist.
            {
                foreach (AudioSource sequenceAudioSource in sequenceAudioSources) // Iterates through every assigned sequence sound.
                {
                    if (sequenceAudioSource == null) // Checks whether the current audio reference is empty.
                    {
                        continue; // Skips the missing reference.
                    }

                    sequenceAudioSource.Stop(); // Stops sounds that should not continue into gameplay.
                }
            }

            if (sleepParalysisFigure != null) // Checks whether the temporary figure exists.
            {
                sleepParalysisFigure.SetActive(false); // Guarantees that the figure is removed.
            }

            RestoreGameplayCameraPose(); // Restores the normal standing Player-camera pose.

            if (playerLookLimiter != null) // Checks whether the constrained-look controller exists.
            {
                playerLookLimiter.EndLimitedLook(); // Ends limited looking and synchronizes normal camera input.
            }

            StartCoroutine(RestoreFullControlNextFrame()); // Restores gameplay after the Space key has been released.

            if (logSkip) // Checks whether development logging is enabled.
            {
                Debug.Log("Bed opening sequence skipped.", this); // Reports the successful skip.
            }
        }

        private void RestoreGameplayCameraPose() // Restores cameras without producing a black frame or duplicated listener.
        {
            if (gameplayCamera == null) // Checks whether the gameplay camera is missing.
            {
                Debug.LogWarning("BedOpeningSkipController: Gameplay Camera is not assigned.", this); // Reports the missing required camera.
                return; // Stops camera restoration safely.
            }

            gameplayCamera.transform.localPosition = gameplayCameraLocalPosition; // Restores the normal local camera position.
            gameplayCamera.transform.localRotation = gameplayCameraLocalRotation; // Restores the normal local camera rotation.
            gameplayCamera.fieldOfView = gameplayCameraFieldOfView; // Restores the normal field of view.

            AudioListener cutsceneListener = cutsceneCamera != null // Checks whether a cutscene camera exists.
                ? cutsceneCamera.GetComponent<AudioListener>() // Finds its Audio Listener.
                : null; // Uses no listener when the camera reference is missing.

            if (cutsceneListener != null) // Checks whether the cutscene listener exists.
            {
                cutsceneListener.enabled = false; // Disables it before enabling the gameplay camera.
            }

            AudioListener gameplayListener = gameplayCamera.GetComponent<AudioListener>(); // Finds the gameplay Audio Listener.

            if (gameplayListener != null) // Checks whether the gameplay listener exists.
            {
                gameplayListener.enabled = true; // Ensures normal gameplay audio is available.
            }

            gameplayCamera.gameObject.SetActive(true); // Enables the restored gameplay camera.

            if (cutsceneCamera != null) // Checks whether the cutscene camera exists.
            {
                cutsceneCamera.gameObject.SetActive(false); // Disables the cutscene camera after the gameplay camera is ready.
            }
        }

        private IEnumerator RestoreFullControlNextFrame() // Delays normal controls so Space does not immediately trigger Jump.
        {
            yield return null; // Waits for the next rendered frame.

            if (playerControlStateController != null) // Checks whether the reusable control controller exists.
            {
                playerControlStateController.SetFullControl(); // Restores movement, looking, interaction, and flashlight input.
            }
        }
    }
}