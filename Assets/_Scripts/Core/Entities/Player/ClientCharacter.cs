using Unity.Netcode;
using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player 
{
    public class ClientCharacter : NetworkBehaviour
    {
        private CameraHandler _cameraHandler;
        private Input _input;

        private IMovable _movable;

        private void Awake()
        {
            _input = GetComponent<Input>();
            _cameraHandler = GetComponent<CameraHandler>();
        }

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
        }

        public void FixedUpdate()
        {
            if (!IsOwner)
                return;

            _cameraHandler.Rotate(_input.GetLookDirection());

            _movable.Move(_input.GetMovementDirection());
        }
    }
}