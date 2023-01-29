using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class ItemDragging : MonoBehaviour
    {
        [SerializeField] private Transform _hands;
        [SerializeField] private float _grabDistance;
        [SerializeField] private LayerMask _grabbableObjectLayer;

        [SerializeField] private Collider _playerCollider;

        private IGrabbable _currentItem;
        private Collider _currentItemCollider;

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * _grabDistance, Color.yellow);
        }

        public void StartDragging()
        {
            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _grabDistance, _grabbableObjectLayer))
            {
                if (hit.collider.TryGetComponent(out IGrabbable item))
                {
                    Physics.IgnoreCollision(_playerCollider, hit.collider, true);
                    _hands.transform.position = hit.transform.position;
                    item.Grab(_hands);

                    _currentItem = item;
                    _currentItemCollider = hit.collider;
                }
            }
        }

        public void StopDragging()
        {
            if (_currentItem == null)
                return;

            _currentItem.Drop();
            Physics.IgnoreCollision(_playerCollider, _currentItemCollider, false);

            _currentItem = null;
            _currentItemCollider = null;
        }
    }
}
