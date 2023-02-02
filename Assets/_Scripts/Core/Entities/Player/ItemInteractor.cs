using InkeepersKeep.Core.Entities.Items;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class ItemInteractor : MonoBehaviour
    {
        [Header("Interaction")]
        [SerializeField] private float      _maxItemInteractionDistance;
        [SerializeField] private Transform  _playerCursor;

        [Header("Item moving")]
        [SerializeField] private float      _interpolationSpeed = 5f;

        [Header("Item Drop")]
        [SerializeField] private float      _dropForce = 5f;
        [SerializeField] private float      _dropMinDistance = 0.2f;

        /* Some variables used for moving item */
        private bool                        _isHoldingItem;
        private Transform                   _hoveringItem;
        private Rigidbody                   _hoveringItemRigidBody;

        private void FixedUpdate()
        {
            if(!_isHoldingItem)
            {
                bool res = RaycastFacingItemObject();
               
                /* In the future we will do there some logic to show player item data */
            } else
            {
                MoveHoldItem();
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

            _playerCursor.transform.position = hit.point;
            _hoveringItem = item.transform;
            return true;
        }

        public void MoveHoldItem()
        {
            Vector3 newPosition = Vector3.Lerp(_hoveringItemRigidBody.transform.position, _playerCursor.position, Time.deltaTime * _interpolationSpeed);
            _hoveringItemRigidBody.MovePosition(newPosition);
        }

        public void GrabItem()
        {
            if (_hoveringItem == null) return;
            
            _isHoldingItem = true;

            if (!_hoveringItem.TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) return;

            _hoveringItemRigidBody = rigidbody;
            _hoveringItemRigidBody.useGravity = false;
            _hoveringItemRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void DropItem()
        {
            if (_hoveringItem == null) return;

            _hoveringItemRigidBody.useGravity = true;
            _hoveringItemRigidBody.constraints = RigidbodyConstraints.None;

            if (Vector3.Distance(_hoveringItemRigidBody.position, _playerCursor.position) > _dropMinDistance)
            {
                Vector3 moveDirection = (_playerCursor.position - _hoveringItemRigidBody.position);
                _hoveringItemRigidBody.velocity = moveDirection * _dropForce;
            }
            else
            {
                _hoveringItemRigidBody.velocity = Vector3.zero;
            }

            _isHoldingItem = false;
            _hoveringItem = null;
        }
    }
}
