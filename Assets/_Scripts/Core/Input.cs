using UnityEngine;

namespace InkeepersKeep.Core
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private Controls _controls;
        public Controls Controls => _controls;

        public void Initialize() => _controls = new Controls();
        public void Enable() => _controls.Enable();
        public void Disable() => _controls.Disable();

        public Vector2 GetMovementDirection() => _controls.Player.Movement.ReadValue<Vector2>();
        public Vector2 GetLookDirection() => _controls.Player.LookDirection.ReadValue<Vector2>();
    }
}
