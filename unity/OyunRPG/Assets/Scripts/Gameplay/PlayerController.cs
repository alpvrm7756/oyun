using UnityEngine;

namespace OyunRPG.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float sprintSpeed = 9f;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float gravity = -20f;

        [Header("Camera")]
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float cameraFollowSpeed = 8f;

        private CharacterController characterController;
        private Vector3 velocity;
        private Transform mainCamera;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            if (Camera.main != null)
            {
                mainCamera = Camera.main.transform;
            }
        }

        private void Update()
        {
            if (!characterController.enabled)
            {
                return;
            }

            HandleMovement();
            HandleCameraFollow();
        }

        private void HandleMovement()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            bool wantsToMove = input.sqrMagnitude > 0.01f;

            Vector3 moveDirection = Vector3.zero;
            if (wantsToMove && mainCamera != null)
            {
                Vector3 forward = mainCamera.forward;
                Vector3 right = mainCamera.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();

                moveDirection = (forward * input.y + right * input.x).normalized;
            }

            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
            Vector3 horizontalVelocity = moveDirection * currentSpeed;

            if (characterController.isGrounded)
            {
                velocity.y = -2f;
                if (Input.GetButtonDown("Jump"))
                {
                    velocity.y = Mathf.Sqrt(2f * -gravity);
                }
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            Vector3 totalMove = horizontalVelocity + Vector3.up * velocity.y;
            characterController.Move(totalMove * Time.deltaTime);

            if (wantsToMove)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        private void HandleCameraFollow()
        {
            if (cameraTarget == null || mainCamera == null)
            {
                return;
            }

            Vector3 targetPosition = Vector3.Lerp(mainCamera.position, cameraTarget.position, Time.deltaTime * cameraFollowSpeed);
            mainCamera.position = targetPosition;
            mainCamera.rotation = Quaternion.Lerp(mainCamera.rotation, cameraTarget.rotation, Time.deltaTime * cameraFollowSpeed);
        }
    }
}
