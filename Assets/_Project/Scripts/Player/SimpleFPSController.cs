using UnityEngine;

namespace Irka.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class SimpleFPSController : MonoBehaviour
    {
        public float moveSpeed = 3.5f;
        public float sprintSpeed = 6f;
        public float mouseSensitivity = 120f; // градусов/сек
        public float jumpHeight = 1.2f;
        public float gravity = -9.81f;

        CharacterController cc;
        Transform cam;
        float pitch;        // наклон камеры по X
        Vector3 velocity;   // вертикальная скорость

        void Start()
        {
            cc = GetComponent<CharacterController>();
            cam = GetComponentInChildren<Camera>().transform;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // ====== Поворот мышью ======
            float mx = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float my = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            transform.Rotate(0f, mx, 0f);
            pitch -= my;
            pitch = Mathf.Clamp(pitch, -85f, 85f);
            cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            // ====== Движение WASD ======
            float h = Input.GetAxisRaw("Horizontal");   // A/D
            float v = Input.GetAxisRaw("Vertical");     // W/S
            Vector3 move = (transform.right * h + transform.forward * v).normalized;
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            cc.Move(move * speed * Time.deltaTime);

            // ====== Прыжок и гравитация ======
            if (cc.isGrounded && velocity.y < 0) velocity.y = -2f;
            if (cc.isGrounded && Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);

            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
        }
    }

}
