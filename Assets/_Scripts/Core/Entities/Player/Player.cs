using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private CameraRotation _camRotation;
        private IMovable _movement;

        public void Initialize()
        {
            _movement = GetComponent<IMovable>();
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
    }
}
