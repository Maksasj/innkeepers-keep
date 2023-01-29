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
        [SerializeField] private CameraRotation _camRotation;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Jumping _jumping;

        [SerializeField] private GameObject _head;
        private IMovable _movement;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Destroy(_head);
                return;
            }

            Initialize();
        }

        public void Initialize()
        {
            _input.Initialize();
            _input.Enable();

            _movement = GetComponent<IMovable>();
            
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

            _camRotation.Rotate();
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;

            if (_movement == null)
                return;

            PlayerMoveServerRpc(_input.GetMovementDirection());

            _movement.Move(_input.GetMovementDirection());
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
                GetComponent<IMovable>().Move(direction);
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (_groundCheck.Check())
                _jumping.Jump();
        }
    }
}
