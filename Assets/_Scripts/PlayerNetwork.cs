using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using InkeepersKeep.Core.Entities;

public class PlayerNetwork : NetworkBehaviour
{
    private Controls _controls;

    private Camera _mainCamera;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        _mainCamera = FindObjectOfType<Camera>();
        _mainCamera.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        _controls = new Controls();

        _controls.Enable();
    }

    public void FixedUpdate()
    {
        if (!IsOwner) return;

        //Vector3 moveDir = new Vector3(0, 0, 0);
        //if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        //if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        //if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        //if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        //float moveSpeed = 3f;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;


        _mainCamera.transform.position = transform.position;
        
        RotateCamera();
        Move(_controls.Player.Movement.ReadValue<Vector2>());
    }

    private float _sensitivity = 10f;

    [SerializeField][Range(0, 90)] private float _minViewDistance = 90f;
    private const float MAX_VIEW_DISTANCE = 90f;

    private float _xRotation = 0f;

    public void RotateCamera()
    {
        Vector2 cursorDelta = _controls.Player.LookDirection.ReadValue<Vector2>();

        float mouseX = cursorDelta.x * _sensitivity * Time.deltaTime;
        float mouseY = cursorDelta.y * _sensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_minViewDistance, MAX_VIEW_DISTANCE);

        _mainCamera.transform.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.up * mouseX);
    }

    [SerializeField] private Rigidbody _rigidbody;
    float _speed = 50f;
    float _velocityPower = 0.5f;

    float _acceleration = 5;
    float _decceleration = 20;

    public void Move(Vector2 direction)
    {
        Vector3 transformedDirection = transform.TransformDirection(new Vector3(direction.x, 0f, direction.y));
        Vector3 targetVelocity = new Vector3(transformedDirection.x * _speed, 0f, transformedDirection.z * _speed);
        Vector3 velocityDifference = targetVelocity - _rigidbody.velocity;
        float accelerationRate = (Mathf.Abs(targetVelocity.magnitude) > 0.01f) ? _acceleration : _decceleration;
        float movementX = Mathf.Pow(Mathf.Abs(velocityDifference.x) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.x);
        float movementZ = Mathf.Pow(Mathf.Abs(velocityDifference.z) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.z);
        _rigidbody.AddForce(new Vector3(movementX, 0.0f, movementZ));
    }
}
