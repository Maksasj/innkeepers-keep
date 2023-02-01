using InkeepersKeep.Core.Shaders;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Items
{
    [RequireComponent(typeof(ItemUI), typeof(Tint))]
    public class Item : MonoBehaviour, IGrabbable, ITintable, IHoverable
    {
        [SerializeField] private ItemUI _ui;
        [SerializeField] private Tint _tint;

        [Header("Physics")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _dropForce = 5f;
        [SerializeField] private float _dropMinDistance = 0.2f;
            
        [Header("Grabbing")]
        [SerializeField] private float _interpolationSpeed = 10f;
        private Transform _grabPoint;
        private bool _isDragging = false;

        [Header("Data")]
        [SerializeField] private ItemData _data;

        private ItemConnector _currentItemConnector;

        private void FixedUpdate()
        {
            if (_grabPoint == null)
                return;

            Vector3 newPosition = Vector3.Lerp(transform.position, _grabPoint.position, Time.deltaTime * _interpolationSpeed);
            _rigidbody.MovePosition(newPosition);
        }

        public void Grab(Transform grabPoint)
        {
            if (_currentItemConnector != null)
            {
                _currentItemConnector.Detach();
                _currentItemConnector = null;
            }

            _grabPoint = grabPoint;
            _rigidbody.useGravity = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            EnableTint();
            _isDragging = true;
        }

        public void Drop()
        {
            if (_currentItemConnector != null)
            {
                _currentItemConnector.Attach(this);
            }
            else
            {
                _rigidbody.useGravity = true;
                _rigidbody.constraints = RigidbodyConstraints.None;

                if (Vector3.Distance(transform.position, _grabPoint.position) > _dropMinDistance)
                {
                    Vector3 moveDirection = (_grabPoint.position - transform.position);
                    _rigidbody.velocity = moveDirection * _dropForce;
                }
                else
                {
                    _rigidbody.velocity = Vector3.zero;
                }
            }

            _grabPoint = null;

            _isDragging = false;
            DisableTint();
        }

        public void Hover()
        {
            if (!_isDragging)
                _ui.ShowItemInfoUI(_data);
            else
                _ui.HideItemInfoUI();

            EnableTint();
        }

        public void Unhover()
        {
            _ui.HideItemInfoUI();

            if (_isDragging)
                return;

            DisableTint();
        }

        public void EnableTint() => _tint.Enable();

        public void DisableTint() => _tint.Disable();

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.TryGetComponent(out ItemConnector itemConnector))
                _currentItemConnector = itemConnector;
        }

        private void OnTriggerStay(Collider collision)
        {
            if (_currentItemConnector != null)
                return;

            if (collision.TryGetComponent(out ItemConnector itemConnector))
                _currentItemConnector = itemConnector;
        }

        private void OnTriggerExit(Collider collision)
        {
            if (collision.TryGetComponent(out ItemConnector itemConnector))
                _currentItemConnector = null;
        }
    }
}
