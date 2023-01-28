using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;

        public void Move(Vector2 direction)
        {
            _rigidbody.velocity = transform.TransformDirection(direction.x * _speed * Time.fixedDeltaTime, _rigidbody.velocity.y, direction.y * _speed * Time.fixedDeltaTime);
        }
    }
}
