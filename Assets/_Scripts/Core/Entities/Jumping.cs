using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class Jumping : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _force;

        private bool _onGround;

        public void OnTriggerEnter(Collider collider) => _onGround = true;
        public void OnTriggerExit(Collider collider) => _onGround = false;

        public void Jump()
        {

            if (!_onGround) return;

            _rigidbody.AddForce(Vector2.up * _force, ForceMode.Impulse);
        }
    }
}
