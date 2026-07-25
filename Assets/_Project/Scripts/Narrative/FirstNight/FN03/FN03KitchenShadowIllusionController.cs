using UnityEngine; // Gives access to Unity engine classes.

namespace Okolitsa.Narrative.FirstNight.FN03 // Keeps the kitchen illusion inside the FN-03 narrative namespace.
{
    [DisallowMultipleComponent] // Prevents duplicate illusion controllers on the same GameObject.
    public sealed class FN03KitchenShadowIllusionController : MonoBehaviour // Removes the staged kitchen shadow when the player genuinely looks into the kitchen.
    {
        [Header("Camera")] // Groups the gameplay-camera reference.
        [SerializeField] private Camera playerCamera; // Provides the position and forward direction used for the visibility ray.

        [Header("Illusion References")] // Groups the staged shadow and reveal target.
        [SerializeField] private GameObject shadowCasterRoot; // Stores the grandmother-shaped object that casts the staged shadow.
        [SerializeField] private Collider kitchenRevealTarget; // Stores the invisible collider located inside the kitchen.

        [Header("Visibility Check")] // Groups raycast settings.
        [SerializeField] private LayerMask visibilityMask; // Defines which architecture and reveal-target layers may block or receive the ray.
        [SerializeField] private float maximumRevealDistance = 20f; // Defines the maximum distance at which the kitchen may be confirmed.
        [SerializeField] private bool requireActiveGameplayCamera = true; // Prevents inactive cutscene cameras from resolving the illusion.

        [Header("Starting State")] // Groups authored starting-state settings.
        [SerializeField] private bool showShadowOnStart = true; // Defines whether the grandmother shadow begins visible.

        [Header("Debug")] // Groups development diagnostics.
        [SerializeField] private bool drawDebugRay = true; // Draws the camera ray in the Scene view during Play Mode.
        [SerializeField] private bool logReveal = true; // Writes successful resolution to the Console.

        [Header("Runtime State")] // Shows the current illusion state during Play Mode.
        [SerializeField] private bool hasResolved; // Stores whether the player has already confirmed that the kitchen is empty.

        public bool HasResolved => hasResolved; // Exposes the resolved state as read-only information.

        private void Awake() // Runs when the narrative illusion is initialized.
        {
            hasResolved = false; // Resets the runtime state for the current scene load.

            if (shadowCasterRoot != null) // Checks whether the staged shadow object is assigned.
            {
                shadowCasterRoot.SetActive(showShadowOnStart); // Applies the authored starting visibility.
            }
            else // Runs when the required shadow reference is missing.
            {
                Debug.LogWarning($"{name}: Shadow Caster Root is not assigned.", this); // Reports the missing shadow reference.
            }

            if (kitchenRevealTarget != null) // Checks whether the invisible kitchen target is assigned.
            {
                kitchenRevealTarget.enabled = true; // Ensures that the reveal target can receive the camera ray.
            }
            else // Runs when the required target reference is missing.
            {
                Debug.LogWarning($"{name}: Kitchen Reveal Target is not assigned.", this); // Reports the missing target reference.
            }

            if (playerCamera == null) // Checks whether the gameplay-camera reference is missing.
            {
                Debug.LogWarning($"{name}: Player Camera is not assigned.", this); // Reports the missing camera reference.
            }
        }

        private void Update() // Runs once per rendered frame.
        {
            if (hasResolved) // Checks whether the illusion has already finished.
            {
                return; // Prevents repeated raycasts after the one-time reveal.
            }

            if (playerCamera == null) // Checks whether the gameplay camera is unavailable.
            {
                return; // Stops safely because no view direction can be evaluated.
            }

            if (shadowCasterRoot == null) // Checks whether the staged shadow object is unavailable.
            {
                return; // Stops safely because no illusion can be removed.
            }

            if (kitchenRevealTarget == null) // Checks whether the reveal target is unavailable.
            {
                return; // Stops safely because the kitchen cannot be confirmed.
            }

            if (requireActiveGameplayCamera && !playerCamera.isActiveAndEnabled) // Checks whether the normal gameplay camera is currently rendering.
            {
                return; // Prevents the opening cutscene from accidentally resolving the illusion.
            }

            Ray visibilityRay = new Ray( // Creates one ray from the center of the gameplay camera.
                playerCamera.transform.position, // Uses the gameplay camera as the ray origin.
                playerCamera.transform.forward); // Uses the center-screen viewing direction.

            if (drawDebugRay) // Checks whether development visualization is enabled.
            {
                Debug.DrawRay( // Draws the visibility ray in the Scene view.
                    visibilityRay.origin, // Starts the line at the camera.
                    visibilityRay.direction * maximumRevealDistance, // Extends the line to the configured maximum distance.
                    Color.cyan); // Uses cyan so the ray is easy to identify.
            }

            bool hitSomething = Physics.Raycast( // Checks the first valid collider reached by the camera ray.
                visibilityRay, // Uses the camera-centered ray.
                out RaycastHit hit, // Stores information about the first collider reached.
                maximumRevealDistance, // Limits how far the visibility check may travel.
                visibilityMask, // Includes blocking architecture and the kitchen reveal target.
                QueryTriggerInteraction.Collide); // Allows the trigger-based kitchen target to receive the ray.

            if (!hitSomething) // Checks whether the ray reached no included collider.
            {
                return; // Keeps the shadow visible.
            }

            if (hit.collider != kitchenRevealTarget) // Checks whether architecture was reached before the kitchen target.
            {
                return; // Keeps the shadow visible while walls or corners still block the kitchen.
            }

            ResolveIllusion(); // Removes the staged shadow after the kitchen becomes genuinely visible.
        }

        public void ResolveIllusion() // Completes the one-time kitchen-shadow illusion.
        {
            if (hasResolved) // Checks whether the illusion was already resolved.
            {
                return; // Prevents duplicate state changes.
            }

            hasResolved = true; // Stores that the player has confirmed the empty kitchen.

            if (shadowCasterRoot != null) // Checks whether the staged shadow object exists.
            {
                shadowCasterRoot.SetActive(false); // Removes the grandmother shadow immediately.
            }

            if (kitchenRevealTarget != null) // Checks whether the invisible target exists.
            {
                kitchenRevealTarget.enabled = false; // Prevents further reveal-ray hits.
            }

            if (logReveal) // Checks whether development logging is enabled.
            {
                Debug.Log("FN-03 kitchen shadow illusion resolved.", this); // Reports the successful one-time reveal.
            }
        }

#if UNITY_EDITOR // Includes manual testing commands only inside the Unity Editor.
        [ContextMenu("Debug/Resolve Kitchen Shadow")] // Adds a manual command for testing disappearance.
        private void DebugResolveIllusion() // Supports shadow-removal testing without walking through the apartment.
        {
            ResolveIllusion(); // Removes the staged shadow.
        }

        [ContextMenu("Debug/Reset Kitchen Shadow")] // Adds a manual command for restoring the staged illusion.
        private void DebugResetIllusion() // Supports repeated placement and lighting tests.
        {
            hasResolved = false; // Restores the unresolved runtime state.

            if (shadowCasterRoot != null) // Checks whether the staged shadow object exists.
            {
                shadowCasterRoot.SetActive(true); // Restores the shadow caster.
            }

            if (kitchenRevealTarget != null) // Checks whether the reveal target exists.
            {
                kitchenRevealTarget.enabled = true; // Restores raycast detection.
            }
        }
#endif // Ends the Unity Editor-only section.
    }
}