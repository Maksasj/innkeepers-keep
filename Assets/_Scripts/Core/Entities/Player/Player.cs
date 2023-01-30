using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

namespace InkeepersKeep.Core.Entities.Player
{
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(Jumping))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Jumping _jumping;

        [SerializeField] private GameObject _headCamera;
        private CameraRotation _camRotation;
        private IMovable _movable;

        private Vector2 _movementDirection;
        private Vector2 _deltaMouse;

        private void Awake() 
        { 
            _movable = GetComponent<IMovable>();
            _camRotation = GetComponentInChildren<CameraRotation>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Destroy(_headCamera);
                return;
            }

            Initialize();
        }

        public void Initialize()
        {
            _input.Initialize();
            _input.Enable();
            
            _input.Controls.Player.Jump.performed += Jump;
        }

        public void Disable()
        {
            _input.Controls.Player.Jump.performed -= Jump;
        }

        private void Update()
        {
            if (!IsOwner)
                return;

            _deltaMouse = _input.GetDeltaMouse();
            _camRotation.Rotate(_deltaMouse);

            if (_deltaMouse != Vector2.zero)
            {
                //_camRotation.Rotate(_deltaMouse);
                PlayerRotateCameraServerRpc(_deltaMouse);
            }
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;

            if (_movable == null)
                return;

            _movementDirection = _input.GetMovementDirection();

            if (_movementDirection != Vector2.zero)
            {
                PlayerMoveServerRpc(_input.GetMovementDirection());
                _movable.Move(_input.GetMovementDirection());
            }
        }

        [ServerRpc]
        private void PlayerMoveServerRpc(Vector3 direction)
        {
            PlayerMoveClientRpc(direction);
        }

        [ClientRpc]
        private void PlayerMoveClientRpc(Vector3 direction)
        {
            if (!IsOwner)
                _movable.Move(direction);
        }

        [ServerRpc]
        private void PlayerRotateCameraServerRpc(Vector2 deltaMouse)
        {
            PlayerRotateCameraClientRpc(deltaMouse);
        }

        [ClientRpc]
        private void PlayerRotateCameraClientRpc(Vector2 deltaMouse)
        {
            if (!IsOwner)
                _camRotation.Rotate(deltaMouse);
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (_groundCheck.Check())
                _jumping.Jump();
        }
    }
}
