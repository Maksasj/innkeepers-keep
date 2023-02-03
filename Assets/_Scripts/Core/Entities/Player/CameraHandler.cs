using Unity.Netcode;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class CameraHandler : NetworkBehaviour
    {
        [SerializeField] private Camera _activeCamera;

        [SerializeField] Transform _bodyTransform;
        [SerializeField][Range(0.1f, 20)] private float _sensitivity;

        private const float MAX_VIEW_ANGLE = 90f;
        [SerializeField][Range(0, MAX_VIEW_ANGLE)] private float _minViewAngle = 90f;

        private float _xRotation = 0f;

        public void Initialize()
        {
            if (!IsOwner) return;

            _activeCamera.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            _activeCamera.gameObject.SetActive(true);

            /* Lets set this game object as parent for camera */
            _activeCamera.transform.SetParent(transform);
            _activeCamera.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            _activeCamera.nearClipPlane = 0.5f;
        }

        public virtual void Rotate(Vector2 cursorDelta)
        {
            if (!IsOwner) return;

            float mouseX = cursorDelta.x * _sensitivity * Time.deltaTime;
            float mouseY = cursorDelta.y * _sensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -_minViewAngle, MAX_VIEW_ANGLE);

            _bodyTransform.transform.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
    }
}
