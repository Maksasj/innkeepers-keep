using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InkeepersKeep.Core.Entities.Player 
{
    public class ClientCharacter : NetworkBehaviour
    {
        [SerializeField] private CameraHandler _cameraHandler;
        [SerializeField] private ItemInteractor _itemInteractor;
        [SerializeField] private Input _input;
        [SerializeField] private Jumping _jumping;

        private IMovable _movable;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) 
                return;

            _input.Initialize();
            _input.Enable();

            _movable = GetComponent<IMovable>();

            _cameraHandler.Initialize();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _input.Controls.Player.Interaction.performed += TryInteract;
            _input.Controls.Player.Interaction.canceled += TryInteract;

            _input.Controls.Player.Jump.performed += TryJump;
        }

        public override void OnNetworkDespawn()
        {
            _input.Controls.Player.Interaction.performed -= TryInteract;
            _input.Controls.Player.Interaction.canceled -= TryInteract;

            _input.Controls.Player.Jump.performed -= TryJump;
        }

        public void FixedUpdate()
        {
            if (!IsOwner)
                return;

            _cameraHandler.Rotate(_input.GetLookDirection());

            _movable.Move(_input.GetMovementDirection());
        }

        private void TryJump(InputAction.CallbackContext ctx)
        {
            _jumping.Jump();
        }

        private void TryInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _itemInteractor.GrabItem();

            if (ctx.canceled)
                _itemInteractor.DropItem();
        }
    }
}