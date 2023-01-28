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
            _rigidbody.velocity = transform.TransformDirection(new Vector3(direction.x * _speed, _rigidbody.velocity.y, direction.y * _speed));
        }
    }
}
