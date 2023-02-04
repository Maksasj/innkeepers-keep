using InkeepersKeep.Core.Entities.Items;
using InkeepersKeep.Network;
using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Entities.Player
{
    public class ItemInteractor : NetworkBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private LayerMask _grabbableItemLayer;
        [SerializeField] private float _maxItemInteractionDistance;
        [SerializeField] private Transform _playerCursor;

        [Header("Item moving")]
        [SerializeField] private float _interpolationSpeed = 5f;

        [Header("Item Drop")]
        [SerializeField] private float _dropForce = 5f;
        [SerializeField] private float _dropMinDistance = 0.2f;

        /* Some variables used for moving item */
        private bool _isHoldingItem;
        private Transform _hoveringItem;
        private Rigidbody _hoveringItemRigidbody;

        private void FixedUpdate()
        {
            if (!_isHoldingItem)
            {
                bool res = RaycastFacingItemObject();
               
                /* In the future we will do there some logic to show player item data */
            }
            else
            {
                MoveHoldItem();
            }
        }

        private bool RaycastFacingItemObject()
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            Debug.DrawRay(transform.position, transform.forward);

            if (!Physics.Raycast(ray, out hit, _maxItemInteractionDistance, _grabbableItemLayer))
            {
                _hoveringItem = null;

                return false;
            }

            if (!hit.collider.TryGetComponent(out ItemObject item))
            {
                _hoveringItem = null;

                return false;
            }

            _playerCursor.transform.position = hit.point;
            _hoveringItem = item.transform;
            return true;
        }

        private void MoveHoldItem()
        {
            Vector3 newPosition = Vector3.Lerp(_hoveringItemRigidbody.transform.position, _playerCursor.position, Time.deltaTime * _interpolationSpeed);
            _hoveringItemRigidbody.MovePosition(newPosition);
        }

        public void GrabItem()
        {
            if (_hoveringItem == null)
                    return;

            if (!_hoveringItem.TryGetComponent(out Rigidbody rigidbody))
                return;

            if (_hoveringItem.TryGetComponent(out NetworkPhysicalObject networkPhysicalObject))
                if (!networkPhysicalObject.IsOwner)
                    networkPhysicalObject.TakeOwnership(OwnerClientId);

            _isHoldingItem = true;

            _hoveringItemRigidbody = rigidbody;
            _hoveringItemRigidbody.useGravity = false;
            _hoveringItemRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void DropItem()
        {
            if (_hoveringItem == null)
                return;

            if (_hoveringItemRigidbody == null)
                return;

            _hoveringItemRigidbody.useGravity = true;
            _hoveringItemRigidbody.constraints = RigidbodyConstraints.None;

            if (Vector3.Distance(_hoveringItemRigidbody.position, _playerCursor.position) > _dropMinDistance)
            {
                Vector3 moveDirection = _playerCursor.position - _hoveringItemRigidbody.position;
                _hoveringItemRigidbody.velocity = moveDirection * _dropForce;
            }
            else
            {
                _hoveringItemRigidbody.velocity = Vector3.zero;
            }

            _isHoldingItem = false;
            _hoveringItem = null;
        }
    }
}
