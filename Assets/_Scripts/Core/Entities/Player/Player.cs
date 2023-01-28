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
        [SerializeField] private Camera _headCam;
        private IMovable _movement;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                Destroy(_headCam);
                Destroy(this);
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
            _camRotation.Rotate();
        }

        private void FixedUpdate()
        {
            if (_movement == null)
                return;

            _movement.Move(_input.GetMovementDirection());
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (_groundCheck.Check())
                _jumping.Jump();
        }
    }
}
