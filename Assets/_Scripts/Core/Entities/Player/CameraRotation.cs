using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private Transform _playerTransform;

        [SerializeField][Range(0.1f, 1)] private float _sensitivity;

        [SerializeField][Range(0, 90)] private float _minViewDistance = 90f;
        private const float MAX_VIEW_DISTANCE = 90f;

        private float _verticalRotation = 0f;
        private float _horizontalRotation = 0f;

        public void Rotate()
        {
            Vector2 inputAxis = _input.GetLookDirection();

            _verticalRotation = inputAxis.y * -_sensitivity;
            _horizontalRotation = inputAxis.x * _sensitivity;

            _verticalRotation = Mathf.Clamp(_verticalRotation, -_minViewDistance, MAX_VIEW_DISTANCE);

            transform.localEulerAngles = Vector3.right * _verticalRotation;
            _playerTransform.transform.localEulerAngles = Vector3.up * _horizontalRotation;
        }
    }   
}
