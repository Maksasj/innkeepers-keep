using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private Transform _playerTransform;

        [SerializeField][Range(0.1f, 20)] private float _sensitivity;

        [SerializeField][Range(0, 90)] private float _minViewDistance = 90f;
        private const float MAX_VIEW_DISTANCE = 90f;

        private float _xRotation = 0f;

        public void Rotate()
        {
            Vector2 cursorDelta = _input.GetLookDirection();

            float mouseX = cursorDelta.x * _sensitivity * Time.deltaTime;
            float mouseY = cursorDelta.y * _sensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -_minViewDistance, MAX_VIEW_DISTANCE);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            _playerTransform.Rotate(Vector3.up * mouseX);
        }
    }   
}
