using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class ItemInteractor : MonoBehaviour
    {
        [SerializeField] private float _maxItemInteractionDistance;
        [SerializeField] private Transform _playerCursor;

        [Header("Grabbing")]
        [SerializeField] private float _interpolationSpeed = 10f;

        private bool _isGrabbingItem;
        
        private Transform _hoveringItem;
        private Rigidbody _hoveringItemRigidBody;

        private void FixedUpdate()
        {
            if(!_isGrabbingItem)
            {
                bool res = RaycastFacingItemObject();
            } else
            {
                Debug.Log("Grabbing item object !");

                Vector3 newPosition = Vector3.Lerp(_hoveringItem.position, _playerCursor.position, Time.deltaTime * _interpolationSpeed);
                _hoveringItemRigidBody.MovePosition(newPosition);
            }
        }

        private bool RaycastFacingItemObject()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            Debug.DrawRay(transform.position, transform.forward);

            if (!Physics.Raycast(ray, out hit, _maxItemInteractionDistance))
            {
                _hoveringItem = null;

                return false;
            }

            if (!hit.collider.TryGetComponent<ItemObject>(out ItemObject item))
            {
                _hoveringItem = null;

                return false;
            }

            _playerCursor.transform.position = hit.transform.position;
            _hoveringItem = item.transform;
            Debug.Log("Facing item object !");
            return true;
        }

        public void GrabItem()
        {
            if (_hoveringItem == null) return;
            
            _isGrabbingItem = true;

            if (!_hoveringItem.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) return;

            _hoveringItemRigidBody = rigidbody;
            _hoveringItemRigidBody.useGravity = false;
            _hoveringItemRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void DropItem()
        {
            _hoveringItemRigidBody.useGravity = true;
            _hoveringItemRigidBody.constraints = RigidbodyConstraints.None;

            _isGrabbingItem = false;
            _hoveringItem = null;
        }
    }
}
