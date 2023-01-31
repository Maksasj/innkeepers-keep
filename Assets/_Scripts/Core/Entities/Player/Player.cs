using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using InkeepersKeep.Core.Network;

namespace InkeepersKeep.Core.Entities.Player
{
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(Jumping))]
    public class Player : NetworkBehaviour
    {
        [SerializeField] private GroundCheck _groundCheck;
        private NetworkMovement _networkMovement;
        private Input _input;
        private Jumping _jumping;

        [SerializeField] private GameObject _headCamera;

        private Vector2 _movementDirection;
        private Vector2 _deltaMouse;

        private void Awake() 
        {
            _networkMovement = GetComponent<NetworkMovement>();
            _jumping = GetComponent<Jumping>();
            _input = GetComponent<Input>();

            _input.Initialize();
            _input.Enable();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
                Destroy(_headCamera);
        }

        private void Update()
        {
            _deltaMouse = _input.GetDeltaMouse();
            _movementDirection = _input.GetMovementDirection();

            if (IsClient && IsLocalPlayer)
                _networkMovement.ProcessLocalPlayerMovement(_movementDirection, _deltaMouse);
            else
                _networkMovement.ProcessSimulatedPlayerMovement();
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (_groundCheck.Check())
            {
                _jumping.Jump();
            }
        }
    }
}
