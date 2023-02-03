using Unity.Netcode;
using UnityEngine;
using InkeepersKeep.Core.Entities;

namespace InkeepersKeep.Core.Entities.Player 
{
    public class ClientCharacter : NetworkBehaviour
    {
        [SerializeField] private CameraHandler _cameraHandler;
        [SerializeField] private ItemInteractor _itemInteractor;
        [SerializeField] private Input _input;

        private IMovable _movable;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) 
                return;

            _input.Initialize();
            _input.Enable();

            _movable = GetComponent<IMovable>();

            _cameraHandler.Initialize(Camera.main);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            _input.Controls.Player.Interaction.performed += TryInteract;
            _input.Controls.Player.Interaction.canceled += TryInteract;
        }

        public void FixedUpdate()
        {
            if (!IsOwner)
                return;

            _cameraHandler.Rotate(_input.GetLookDirection());

            _movable.Move(_input.GetMovementDirection());
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