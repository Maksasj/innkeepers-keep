using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _speed;

        public void Move(Vector2 direction)
        {
            _rigidBody.velocity = new Vector3(direction.x * _speed, _rigidBody.velocity.y, direction.y * _speed);
        }
    }
}
