using System.Collections; // Gives access to IEnumerator for the handoff coroutine.
using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Narrative.CameraSystem // Keeps narrative camera tools inside a dedicated namespace.
{
    public sealed class CameraPoseHandoff : MonoBehaviour // Blends the gameplay camera from a cutscene pose back to its normal local pose.
    {
        [Header("Camera References")] // Groups required camera references in the Inspector.
        [SerializeField] private Camera cutsceneCamera; // Stores the camera used by the authored Timeline shot.
        [SerializeField] private Camera gameplayCamera; // Stores the normal camera attached to the Player.

        [Header("Blend Settings")] // Groups transition timing and interpolation settings.
        [SerializeField] private float blendDuration = 1.25f; // Defines how long the gameplay camera takes to return to its normal local pose.
        [SerializeField] private AnimationCurve blendCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Defines the smoothness of the transition.
        [SerializeField] private bool blendFieldOfView = true; // Defines whether camera field of view is also blended.

        [Header("Debug")] // Groups optional development logging.
        [SerializeField] private bool logHandoff = true; // Defines whether the handoff writes status messages to the Console.

        private Vector3 gameplayLocalPosition; // Stores the normal local position of the gameplay camera.
        private Quaternion gameplayLocalRotation; // Stores the normal local rotation of the gameplay camera.
        private float gameplayFieldOfView; // Stores the normal field of view of the gameplay camera.
        private Coroutine handoffCoroutine; // Stores the currently running handoff coroutine.

        private void Awake() // Runs when the scene object is initialized.
        {
            if (gameplayCamera == null) // Checks whether the gameplay camera reference is missing.
            {
                Debug.LogWarning("CameraPoseHandoff: Gameplay Camera is not assigned."); // Reports the missing reference.
                return; // Stops initialization because no gameplay pose can be stored.
            }

            gameplayLocalPosition = gameplayCamera.transform.localPosition; // Saves the camera's normal local position inside the Player.
            gameplayLocalRotation = gameplayCamera.transform.localRotation; // Saves the camera's normal local rotation inside the Player.
            gameplayFieldOfView = gameplayCamera.fieldOfView; // Saves the camera's normal field of view.
        }

        public void BeginHandoff() // Provides a parameterless method suitable for a Timeline Signal Receiver.
        {
            if (cutsceneCamera == null) // Checks whether the cutscene camera reference is missing.
            {
                Debug.LogWarning("CameraPoseHandoff: Cutscene Camera is not assigned."); // Reports the missing reference.
                return; // Stops safely.
            }

            if (gameplayCamera == null) // Checks whether the gameplay camera reference is missing.
            {
                Debug.LogWarning("CameraPoseHandoff: Gameplay Camera is not assigned."); // Reports the missing reference.
                return; // Stops safely.
            }

            if (handoffCoroutine != null) // Checks whether another handoff is already running.
            {
                StopCoroutine(handoffCoroutine); // Stops the previous transition to prevent competing camera updates.
            }

            handoffCoroutine = StartCoroutine(PerformHandoff()); // Starts the seamless camera transition.
        }

        private IEnumerator PerformHandoff() // Copies the cutscene pose and blends back to the gameplay pose.
        {
            Transform gameplayTransform = gameplayCamera.transform; // Caches the gameplay camera transform.
            Transform cutsceneTransform = cutsceneCamera.transform; // Caches the cutscene camera transform.

            gameplayTransform.position = cutsceneTransform.position; // Copies the final cutscene-camera world position.
            gameplayTransform.rotation = cutsceneTransform.rotation; // Copies the final cutscene-camera world rotation.

            if (blendFieldOfView) // Checks whether field of view should also match during the cut.
            {
                gameplayCamera.fieldOfView = cutsceneCamera.fieldOfView; // Copies the cutscene field of view before switching cameras.
            }

            AudioListener cutsceneListener = cutsceneCamera.GetComponent<AudioListener>(); // Finds the cutscene camera Audio Listener.

            if (cutsceneListener != null) // Checks whether the cutscene camera has an Audio Listener.
            {
                cutsceneListener.enabled = false; // Disables it before enabling the gameplay camera to prevent duplicate listeners.
            }

            gameplayCamera.gameObject.SetActive(true); // Enables the already aligned gameplay camera.
            cutsceneCamera.gameObject.SetActive(false); // Disables the cutscene camera after the gameplay camera is ready.

            Vector3 startingLocalPosition = gameplayTransform.localPosition; // Stores the temporary offset required to match the cutscene pose.
            Quaternion startingLocalRotation = gameplayTransform.localRotation; // Stores the temporary local rotation required to match the cutscene pose.
            float startingFieldOfView = gameplayCamera.fieldOfView; // Stores the field of view used at the beginning of the blend.

            if (logHandoff) // Checks whether development logging is enabled.
            {
                Debug.Log("Camera handoff started."); // Reports that the transition has begun.
            }

            float elapsedTime = 0f; // Starts the transition timer at zero.

            while (elapsedTime < blendDuration) // Continues until the configured transition duration has passed.
            {
                elapsedTime += Time.deltaTime; // Advances the transition timer using frame time.

                float normalizedTime = blendDuration > 0f ? elapsedTime / blendDuration : 1f; // Converts elapsed time into a zero-to-one value.
                normalizedTime = Mathf.Clamp01(normalizedTime); // Prevents the value from exceeding the valid range.

                float evaluatedTime = blendCurve.Evaluate(normalizedTime); // Applies the configured easing curve.

                gameplayTransform.localPosition = Vector3.LerpUnclamped( // Blends the camera's local position.
                    startingLocalPosition, // Uses the pose copied from the cutscene as the starting point.
                    gameplayLocalPosition, // Uses the normal Player-camera position as the destination.
                    evaluatedTime); // Uses the eased transition progress.

                gameplayTransform.localRotation = Quaternion.SlerpUnclamped( // Blends the camera's local rotation.
                    startingLocalRotation, // Uses the cutscene-matched rotation as the starting point.
                    gameplayLocalRotation, // Uses the normal gameplay rotation as the destination.
                    evaluatedTime); // Uses the eased transition progress.

                if (blendFieldOfView) // Checks whether field of view should be blended.
                {
                    gameplayCamera.fieldOfView = Mathf.LerpUnclamped( // Blends the camera field of view.
                        startingFieldOfView, // Uses the cutscene field of view as the starting value.
                        gameplayFieldOfView, // Uses the normal gameplay field of view as the destination.
                        evaluatedTime); // Uses the eased transition progress.
                }

                yield return null; // Waits until the next rendered frame.
            }

            gameplayTransform.localPosition = gameplayLocalPosition; // Applies the exact normal local camera position.
            gameplayTransform.localRotation = gameplayLocalRotation; // Applies the exact normal local camera rotation.

            if (blendFieldOfView) // Checks whether field of view was included.
            {
                gameplayCamera.fieldOfView = gameplayFieldOfView; // Restores the exact normal gameplay field of view.
            }

            handoffCoroutine = null; // Marks the transition as completed.

            if (logHandoff) // Checks whether development logging is enabled.
            {
                Debug.Log("Camera handoff completed."); // Reports that the transition has finished.
            }
        }
    }
}