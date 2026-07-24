using UnityEngine; // Gives access to Unity engine classes.

namespace Irka.Player // Preserves the existing namespace so current scene references remain compatible.
{
    [RequireComponent(typeof(CharacterController))] // Ensures that the player always has a CharacterController.
    public class SimpleFPSController : MonoBehaviour // Handles first-person movement, mouse look, jumping, and gravity.
    {
        [Header("Movement")] // Groups movement settings in the Inspector.
        [SerializeField] private float moveSpeed = 3.5f; // Defines normal walking speed.
        [SerializeField] private float sprintSpeed = 6f; // Defines movement speed while sprinting.
        [SerializeField] private float jumpHeight = 1.2f; // Defines the height of a player jump.
        [SerializeField] private float gravity = -9.81f; // Defines downward acceleration.

        [Header("Camera Look")] // Groups camera-look settings in the Inspector.
        [SerializeField] private float mouseSensitivity = 120f; // Defines mouse-look sensitivity.
        [SerializeField] private float minimumPitch = -85f; // Defines the lowest vertical camera angle.
        [SerializeField] private float maximumPitch = 85f; // Defines the highest vertical camera angle.

        [Header("Control Permissions")] // Shows the current reusable control permissions.
        [SerializeField] private bool movementEnabled = true; // Defines whether movement, sprinting, and jumping are allowed.
        [SerializeField] private bool lookEnabled = true; // Defines whether mouse-look input is allowed.

        private CharacterController characterController; // Stores the player CharacterController.
        private Transform cameraTransform; // Stores the player camera transform.
        private float pitch; // Stores the current vertical camera angle.
        private Vector3 verticalVelocity; // Stores the current vertical velocity used for gravity and jumping.

        public bool IsMovementEnabled => movementEnabled; // Exposes the current movement permission as read-only state.
        public bool IsLookEnabled => lookEnabled; // Exposes the current look permission as read-only state.

        private void Awake() // Runs when the player object is initialized.
        {
            characterController = GetComponent<CharacterController>(); // Caches the required CharacterController.

            Camera playerCamera = GetComponentInChildren<Camera>(true); // Finds the player camera, including inactive children.

            if (playerCamera != null) // Checks whether a camera was found.
            {
                cameraTransform = playerCamera.transform; // Caches the camera transform.
            }
            else // Runs if no child camera exists.
            {
                Debug.LogWarning("SimpleFPSController: No child Camera was found."); // Reports the missing camera reference.
            }
        }

        private void Start() // Runs before the first gameplay frame.
        {
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to the Game view.
            Cursor.visible = false; // Hides the cursor during first-person gameplay.
        }

        private void Update() // Runs once per frame.
        {
            if (lookEnabled) // Checks whether mouse-look input is currently allowed.
            {
                HandleMouseLook(); // Processes camera and player rotation.
            }

            HandleMovementAndGravity(); // Processes movement permissions, jumping, and gravity.
        }

        private void HandleMouseLook() // Handles horizontal and vertical camera rotation.
        {
            if (cameraTransform == null) // Checks whether the player camera reference is missing.
            {
                return; // Stops safely because camera rotation cannot be applied.
            }

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Reads horizontal mouse movement.
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // Reads vertical mouse movement.

            transform.Rotate(0f, mouseX, 0f); // Rotates the player body horizontally.

            pitch -= mouseY; // Applies inverted vertical mouse movement to the camera pitch.
            pitch = Mathf.Clamp(pitch, minimumPitch, maximumPitch); // Prevents excessive vertical rotation.

            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Applies the vertical camera rotation.
        }

        private void HandleMovementAndGravity() // Handles horizontal movement, sprinting, jumping, and gravity.
        {
            Vector3 horizontalMovement = Vector3.zero; // Starts with no horizontal movement.

            if (movementEnabled) // Checks whether gameplay movement input is currently allowed.
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal"); // Reads A and D input.
                float verticalInput = Input.GetAxisRaw("Vertical"); // Reads W and S input.

                horizontalMovement = (transform.right * horizontalInput + transform.forward * verticalInput).normalized; // Converts input into player-relative movement.

                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed; // Selects sprint or walking speed.

                characterController.Move(horizontalMovement * currentSpeed * Time.deltaTime); // Moves the player horizontally.

                if (characterController.isGrounded && Input.GetButtonDown("Jump")) // Checks whether the grounded player pressed Jump.
                {
                    verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculates the required upward jump velocity.
                }
            }

            if (characterController.isGrounded && verticalVelocity.y < 0f) // Checks whether the player is grounded and moving downward.
            {
                verticalVelocity.y = -2f; // Keeps the CharacterController attached reliably to the ground.
            }

            verticalVelocity.y += gravity * Time.deltaTime; // Applies gravity even when player input is disabled.

            characterController.Move(verticalVelocity * Time.deltaTime); // Applies vertical motion to the player.
        }

        public void SetControlPermissions(bool allowMovement, bool allowLook) // Applies movement and camera-look permissions together.
        {
            movementEnabled = allowMovement; // Stores the requested movement permission.
            lookEnabled = allowLook; // Stores the requested look permission.
        }

        public void SetMovementEnabled(bool isEnabled) // Changes only the movement permission.
        {
            movementEnabled = isEnabled; // Stores the requested movement state.
        }

        public void SetLookEnabled(bool isEnabled) // Changes only the camera-look permission.
        {
            lookEnabled = isEnabled; // Stores the requested camera-look state.
        }

        public void ResetVerticalVelocity() // Clears jump or fall velocity when an external sequence requires it.
        {
            verticalVelocity = Vector3.zero; // Resets the stored vertical velocity.
        }
    }
}