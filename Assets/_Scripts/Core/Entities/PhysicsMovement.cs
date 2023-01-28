using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _decceleration;
        [SerializeField] private float _velocityPower;

        public void Move(Vector2 direction)
        {
            Vector3 transformedDirection = transform.TransformDirection(new Vector3(direction.x, 0f, direction.y));
            Vector3 targetVelocity = new Vector3(transformedDirection.x * _speed, 0f, transformedDirection.z * _speed);
            Vector3 velocityDifference = targetVelocity - _rigidbody.velocity;
            float accelerationRate = (Mathf.Abs(targetVelocity.magnitude) > 0.01f) ? _acceleration : _decceleration;
            float movementX = Mathf.Pow(Mathf.Abs(velocityDifference.x) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.x);
            float movementZ = Mathf.Pow(Mathf.Abs(velocityDifference.z) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.z);

            _rigidbody.AddForce(new Vector3(movementX, _rigidbody.velocity.y, movementZ));
        }
    }
}
