using InkeepersKeep.Core.Entities.Items;
using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Entities.Player
{
    public class ItemInteractor : NetworkBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private LayerMask _interactableItemLayer;
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

            if (!Physics.Raycast(ray, out hit, _maxItemInteractionDistance, _interactableItemLayer))
            {
                _hoveringItem = null;

                return false;
            }

            if (!hit.collider.TryGetComponent(out ItemObject item))
            {
                _hoveringItem = null;

                return false;
            }

            _playerCursor.transform.position = hit.transform.position;
            _hoveringItem = item.transform;
            return true;
        }

        private void MoveHoldItem()
        {
            Vector3 newPosition = Vector3.Lerp(_hoveringItem.position, _playerCursor.position, Time.fixedDeltaTime * _interpolationSpeed);
            _hoveringItemRigidbody.MovePosition(newPosition);
        }

        public void GrabItem()
        {
            if (_hoveringItem == null)
                return;

            if (!_hoveringItem.TryGetComponent(out Rigidbody rigidbody))
                return;

            if (!_hoveringItem.TryGetComponent(out NetworkObject networkObject))
                return;

            _isHoldingItem = true;

            _hoveringItemRigidbody = rigidbody;
            _hoveringItemRigidbody.useGravity = false;
            _hoveringItemRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            if (networkObject.OwnerClientId != OwnerClientId)
                RequestOwnershipServerRpc(networkObject.NetworkObjectId);
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
            _hoveringItemRigidbody = null;
        }

        [ServerRpc]
        public void RequestOwnershipServerRpc(ulong itemId, ServerRpcParams serverRpcParams = default)
        {
            var item = NetworkManager.SpawnManager.SpawnedObjects[itemId];
            item.ChangeOwnership(serverRpcParams.Receive.SenderClientId);
        }
    }
}
