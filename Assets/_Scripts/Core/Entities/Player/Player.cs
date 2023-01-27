using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Input _input;
        private IMovable _movement;

        public void Initialize()
        {
            _movement = GetComponent<IMovable>();
        }

        private void FixedUpdate()
        {
            if (_movement == null)
                return;

            _movement.Move(_input.GetMovementDirection());
        }
    }
}
