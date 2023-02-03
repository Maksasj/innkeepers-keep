using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class Jumping : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force;

        private int _onGround;

        public void OnTriggerEnter(Collider collider) => _onGround += 1;
        public void OnTriggerExit(Collider collider) => _onGround -= 1;

        private bool OnGround()
        {
            return _onGround > 0;
        }

        public void Jump()
        {
            if (!OnGround()) return;

            _rigidbody.AddForce(Vector2.up * _force, ForceMode.Impulse);
        }
    }
}
