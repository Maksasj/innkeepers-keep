using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _playerTransform;

        private float cameraVerticalRotation = 0f;
        private float cameraHorizontalRotation = 0f;

        private void Update()
        {            
            Vector2 inputAxis = _input.GetLookDirection();
            
            cameraVerticalRotation =    inputAxis.y * - 0.3f;
            cameraHorizontalRotation =  inputAxis.x * 0.3f;

            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

            _camera.transform.localEulerAngles = Vector3.right * cameraVerticalRotation;
            _playerTransform.transform.localEulerAngles = Vector3.up * cameraHorizontalRotation;
        }
    }   
}
