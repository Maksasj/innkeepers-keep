using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using InkeepersKeep.Core.Entities;

namespace InkeepersKeep.Core.Entities.Player 
{
    public class ClientCharacter : NetworkBehaviour
    {
        [SerializeField] private CameraHandler _cameraHandler;
        [SerializeField] private Input _input;
        
        private IMovable _movable;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;

            _input.Initialize();
            _input.Enable();

            _movable = GetComponent<IMovable>(); 

            _cameraHandler.Initialize(Camera.main);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            _input.Disable();
        }

        public void FixedUpdate()
        {
            if (!IsOwner) return;

            _cameraHandler.Rotate(_input.GetLookDirection());

            _movable.Move(_input.GetMovementDirection());
        }
    }
}