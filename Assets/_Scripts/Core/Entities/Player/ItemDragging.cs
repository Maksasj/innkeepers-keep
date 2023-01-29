using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class ItemDragging : MonoBehaviour
    {
        [SerializeField] private Transform _hands;
        [SerializeField] private float _grabDistance;
        [SerializeField] private LayerMask _grabbableObjectLayer;

        private IGrabbable _currentItem;

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
                    _hands.transform.position = hit.point;
                    item.Grab(_hands);

                    _currentItem = item;
                }
            }
        }

        public void StopDragging()
        {
            if (_currentItem == null)
                return;

            _currentItem.Drop();
            _currentItem = null;
        }
    }
}
