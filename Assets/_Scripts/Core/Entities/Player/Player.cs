using UnityEngine;
using UnityEngine.InputSystem;

namespace InkeepersKeep.Core.Entities.Player
{
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(Jumping))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private CameraRotation _camRotation;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Jumping _jumping;
        [SerializeField] private PlayerCursor _playerCursor;

        private IMovable _movement;

        public void Initialize()
        {
            _movement = GetComponent<IMovable>();
            _input.Controls.Player.Jump.performed += Jump;

            _input.Controls.Player.Interaction.performed += TryInteract;
            _input.Controls.Player.Interaction.canceled += TryInteract;
        }

        public void Disable()
        {
            _input.Controls.Player.Jump.performed -= Jump;
            _input.Controls.Player.Interaction.performed -= TryInteract;
            _input.Controls.Player.Interaction.canceled -= TryInteract;
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

        private void TryInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
                _playerCursor.StartDragging();

            if (ctx.canceled)
                _playerCursor.StopDragging();
        }
    }
}