using UnityEngine;

namespace OyunRPG.Gameplay
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private Transform focusTarget;
        [SerializeField] private Vector2 sensitivity = new Vector2(3f, 2f);
        [SerializeField] private Vector2 pitchClamp = new Vector2(-35f, 60f);
        [SerializeField] private float followDistance = 5f;
        [SerializeField] private float heightOffset = 2f;

        private float yaw;
        private float pitch;

        private void LateUpdate()
        {
            if (focusTarget == null)
            {
                return;
            }

            yaw += Input.GetAxis("Mouse X") * sensitivity.x;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity.y;
            pitch = Mathf.Clamp(pitch, pitchClamp.x, pitchClamp.y);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 offset = rotation * new Vector3(0f, heightOffset, -followDistance);
            transform.position = focusTarget.position + offset;
            transform.LookAt(focusTarget.position + Vector3.up * heightOffset * 0.5f);
        }
    }
}
