using UnityEngine; // Gives access to Unity engine classes.

namespace Irka.Player // Keeps the limiter together with the existing player systems.
{
    public sealed class PlayerLookLimiter : MonoBehaviour // Handles temporary constrained first-person looking.
    {
        [Header("References")] // Groups required component references.
        [SerializeField] private Transform cameraTransform; // Stores the Player camera transform.
        [SerializeField] private SimpleFPSController fpsController; // Stores the normal FPS look controller.

        [Header("Limited Look Sensitivity")] // Groups mouse sensitivity settings.
        [SerializeField] private float mouseSensitivity = 120f; // Defines mouse sensitivity during constrained looking.

        [Header("Horizontal Range")] // Groups horizontal viewing limits.
        [SerializeField] private float horizontalLimit = 85f; // Allows approximately 85 degrees to each side.

        [Header("Vertical Range")] // Groups asymmetric vertical viewing limits.
        [SerializeField] private float lookUpLimit = 45f; // Defines how far the player may look upward.
        [SerializeField] private float lookDownLimit = 35f; // Defines how far the player may look downward.

        [Header("Runtime State")] // Shows the current limiter state during Play Mode.
        [SerializeField] private bool isActive; // Stores whether constrained looking is currently active.
        [SerializeField] private float currentYawOffset; // Stores horizontal rotation relative to the starting direction.
        [SerializeField] private float currentPitchOffset; // Stores vertical rotation relative to the starting direction.

        private float centerYaw; // Stores the horizontal direction at the start of constrained looking.
        private float centerPitch; // Stores the vertical direction at the start of constrained looking.

        public bool IsActive => isActive; // Exposes the current limiter state as read-only information.

        private void Awake() // Runs when the Player object is initialized.
        {
            if (fpsController == null) // Checks whether the FPS controller was not assigned manually.
            {
                fpsController = GetComponent<SimpleFPSController>(); // Finds the FPS controller on the same Player object.
            }

            if (cameraTransform == null) // Checks whether the camera transform was not assigned manually.
            {
                Camera playerCamera = GetComponentInChildren<Camera>(true); // Searches for the Player camera, including inactive children.

                if (playerCamera != null) // Checks whether a camera was found.
                {
                    cameraTransform = playerCamera.transform; // Stores the found camera transform.
                }
            }

            if (fpsController == null) // Checks whether the required FPS controller is still missing.
            {
                Debug.LogWarning("PlayerLookLimiter: SimpleFPSController is not assigned."); // Reports the missing reference.
            }

            if (cameraTransform == null) // Checks whether the required camera transform is still missing.
            {
                Debug.LogWarning("PlayerLookLimiter: Camera Transform is not assigned."); // Reports the missing reference.
            }
        }

        private void Update() // Runs once per rendered frame.
        {
            if (!isActive) // Checks whether constrained looking is disabled.
            {
                return; // Stops input processing while the limiter is inactive.
            }

            HandleLimitedLook(); // Reads mouse input and applies constrained camera rotation.
        }

        private void HandleLimitedLook() // Handles horizontal, vertical, and diagonal constrained looking.
        {
            if (cameraTransform == null) // Checks whether the camera reference is unavailable.
            {
                return; // Stops safely because camera rotation cannot be applied.
            }

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Reads horizontal mouse movement.
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Reads vertical mouse movement.

            currentYawOffset += mouseX; // Adds horizontal input to the relative yaw angle.
            currentPitchOffset -= mouseY; // Adds inverted vertical input to the relative pitch angle.

            ClampLookOffsetsToEllipse(); // Restricts both angles to one natural oval viewing region.

            transform.rotation = Quaternion.Euler(0f, centerYaw + currentYawOffset, 0f); // Applies horizontal rotation to the Player body.

            float finalPitch = centerPitch + currentPitchOffset; // Combines the starting vertical angle with the current offset.
            cameraTransform.localRotation = Quaternion.Euler(finalPitch, 0f, 0f); // Applies vertical rotation to the Player camera.
        }

        private void ClampLookOffsetsToEllipse() // Restricts diagonal movement using an elliptical viewing boundary.
        {
            float safeHorizontalLimit = Mathf.Max(0.01f, horizontalLimit); // Prevents division by zero for horizontal limits.

            float activeVerticalLimit = currentPitchOffset < 0f // Checks whether the player is currently looking upward.
                ? Mathf.Max(0.01f, lookUpLimit) // Uses the upward limit for negative pitch offsets.
                : Mathf.Max(0.01f, lookDownLimit); // Uses the downward limit for positive pitch offsets.

            float normalizedYaw = currentYawOffset / safeHorizontalLimit; // Converts horizontal rotation into normalized ellipse space.
            float normalizedPitch = currentPitchOffset / activeVerticalLimit; // Converts vertical rotation into normalized ellipse space.

            Vector2 normalizedOffset = new Vector2(normalizedYaw, normalizedPitch); // Combines both normalized look axes.

            if (normalizedOffset.sqrMagnitude <= 1f) // Checks whether the current direction is already inside the ellipse.
            {
                return; // Keeps the current direction unchanged.
            }

            normalizedOffset.Normalize(); // Moves the direction onto the nearest point of the ellipse boundary.

            currentYawOffset = normalizedOffset.x * safeHorizontalLimit; // Converts the clamped horizontal value back into degrees.
            currentPitchOffset = normalizedOffset.y * activeVerticalLimit; // Converts the clamped vertical value back into degrees.
        }

        public void BeginLimitedLook() // Starts constrained looking from the current camera direction.
        {
            if (cameraTransform == null) // Checks whether the camera reference is missing.
            {
                Debug.LogWarning("PlayerLookLimiter: Cannot begin limited look without a camera."); // Reports the missing reference.
                return; // Stops safely.
            }

            centerYaw = transform.eulerAngles.y; // Stores the current Player-facing direction as the horizontal center.
            centerPitch = ConvertToSignedAngle(cameraTransform.localEulerAngles.x); // Stores the current camera pitch as the vertical center.

            currentYawOffset = 0f; // Starts horizontal movement from the current direction.
            currentPitchOffset = 0f; // Starts vertical movement from the current direction.

            if (fpsController != null) // Checks whether the normal FPS controller exists.
            {
                fpsController.SetLookEnabled(false); // Prevents the normal controller from fighting the limiter for camera control.
            }

            isActive = true; // Enables constrained mouse-look processing.
        }

        public void EndLimitedLook() // Ends constrained looking and returns control to the normal FPS controller.
        {
            isActive = false; // Stops constrained mouse-look processing.

            if (fpsController != null) // Checks whether the normal FPS controller exists.
            {
                fpsController.SynchronizeLookState(); // Synchronizes its internal pitch with the current limited-look camera pose.
                fpsController.SetLookEnabled(true); // Restores normal camera-look processing.
            }
        }

        public void SetHorizontalLimit(float value) // Allows Timeline or development tools to change the horizontal range.
        {
            horizontalLimit = Mathf.Clamp(value, 0f, 180f); // Stores a safe horizontal limit.
        }

        public void SetVerticalLimits(float upwardLimit, float downwardLimit) // Allows code to configure both vertical ranges.
        {
            lookUpLimit = Mathf.Clamp(upwardLimit, 0f, 89f); // Stores a safe upward range.
            lookDownLimit = Mathf.Clamp(downwardLimit, 0f, 89f); // Stores a safe downward range.
        }

        private float ConvertToSignedAngle(float angle) // Converts a zero-to-360 Unity angle into a signed angle.
        {
            if (angle > 180f) // Checks whether Unity represents the angle as a wrapped negative value.
            {
                angle -= 360f; // Converts the wrapped angle into the negative range.
            }

            return angle; // Returns the signed angle.
        }

        private void OnDisable() // Runs if the limiter component or Player object is disabled.
        {
            if (!isActive) // Checks whether constrained looking was already inactive.
            {
                return; // Avoids changing unrelated controller state.
            }

            isActive = false; // Clears the runtime limiter state.

            if (fpsController != null) // Checks whether the normal FPS controller exists.
            {
                fpsController.SynchronizeLookState(); // Preserves the camera's current vertical orientation.
                fpsController.SetLookEnabled(true); // Prevents normal look from remaining accidentally disabled.
            }
        }
    }
}