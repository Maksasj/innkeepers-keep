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
        [SerializeField] private ReachSphere _reachSphere;

        public MeshFilter _currentItemMeshFilter;
        public IGrabbable _currentItem;
        private Collider _currentItemCollider;

        public bool _isDragging;

        public void StartDragging()
        {
            Ray ray = new Ray(transform.position, transform.forward);

            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, _grabDistance, _grabbableObjectLayer)) return;
            if (!hit.collider.TryGetComponent(out IGrabbable item)) return;

            Physics.IgnoreCollision(_playerCollider, hit.collider, true);
            _hands.transform.position = hit.transform.position;
            item.Grab(_hands);

            _currentItem = item;
            _currentItemCollider = hit.collider;
            _isDragging = true;

            if (!hit.collider.TryGetComponent<MeshFilter>(out MeshFilter currentItemMeshFilter)) return;
            _currentItemMeshFilter = currentItemMeshFilter;


            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _reachSphere.GetComponent<SphereCollider>().radius);
            foreach (var hitCollider in hitColliders)
            {
                if (!hitCollider.gameObject.TryGetComponent<ItemConnector>(out ItemConnector _itemConnector)) continue;

                _itemConnector.MakeVisible(_currentItemMeshFilter);
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
            _currentItemMeshFilter = null;
            _isDragging = false;

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _reachSphere.GetComponent<SphereCollider>().radius);
            foreach (var hitCollider in hitColliders) {
                if (!hitCollider.gameObject.TryGetComponent<ItemConnector>(out ItemConnector _itemConnector)) continue;

                _itemConnector.MakeInvisible();
            }
        }
    }
}
