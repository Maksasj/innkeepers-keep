using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    public class Item : MonoBehaviour, IGrabbable
    {
        [SerializeField] private GameObject _ItemUI;
        [SerializeField] private Rigidbody _rigidbody;
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
        }

        public void Drop()
        {
            _grabPoint = null;
            _rigidbody.useGravity = true;
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
