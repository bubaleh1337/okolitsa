using UnityEngine;

namespace Okolitsa.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _gravity = -20f;
        [SerializeField] private float _groundedVelocity = -2f;

        private CharacterController _characterController;
        private Vector3 _velocity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            float axisX = Input.GetAxisRaw("Horizontal");
            float axisY = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = transform.right * axisX + transform.forward * axisY;

            if (moveDirection.sqrMagnitude > 1f)
            {
                moveDirection.Normalize();
            }

            _characterController.Move(moveDirection * _moveSpeed * Time.deltaTime);

            if (_characterController.isGrounded && _velocity.y < 0f)
            {
                _velocity.y = _groundedVelocity;
            }

            _velocity.y += _gravity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}