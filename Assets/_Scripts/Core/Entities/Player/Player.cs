using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private CameraRotation _camRotation;
        [SerializeField] private Gravity _gravity;
        private IMovable _movement;

        public void Initialize()
        {
            _movement = GetComponent<IMovable>();
        }

        private void Update()
        {
            _camRotation.Rotate();
            _gravity.Apply();

            if (_movement == null)
                return;

            _movement.Move(_input.GetMovementDirection());
        }
    }
}
