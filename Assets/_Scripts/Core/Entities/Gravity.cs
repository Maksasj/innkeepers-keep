using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterController))]
    public class Gravity : MonoBehaviour
    {
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private CharacterController _characterController;
        [SerializeField][Range(0, 10f)] private float _gravityScale;
        private const float GRAVITY = -9.81f;

        private Vector3 _velocity;

        public void Apply()
        {
            if (_groundCheck.Check() && _velocity.y < 0)
                _velocity.y = -_gravityScale;

            _velocity.y += GRAVITY * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}
