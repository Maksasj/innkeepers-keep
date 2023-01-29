using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    public class Item : MonoBehaviour, IGrabbable
    {
        [SerializeField] private GameObject _ItemUI;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _dropForce = 5f;
        private Transform _grabPoint;

        private void FixedUpdate()
        {
            if (_grabPoint == null)
                return;

            Vector3 newPosition = Vector3.Lerp(transform.position, _grabPoint.position, Time.deltaTime * 10f);
            _rigidbody.MovePosition(newPosition);
        }

        public void Grab(Transform grabPoint)
        {
            _grabPoint = grabPoint;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void Drop()
        {
            _rigidbody.useGravity = true;
            _rigidbody.constraints = RigidbodyConstraints.None;

            Vector3 moveDirection = (_grabPoint.position - transform.position).normalized;

            _rigidbody.velocity = moveDirection * _dropForce;

            _grabPoint = null;
        }

        public void Hover()
        {
            _ItemUI.SetActive(true);
        }

        public void Unhover()
        {
            _ItemUI.SetActive(false);
        }
    }
}
