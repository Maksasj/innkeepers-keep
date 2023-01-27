using UnityEngine;

namespace InkeepersKeep.Core.Entities.Player
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

            Vector2 movementAxis = _input.GetMovementDirection();
            //movementAxis = new Vector2(
            //    movementAxis.x * 10 * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.y),
            //    movementAxis.y * 10 *Mathf.Cos(Mathf.Deg2Rad *  transform.rotation.y));

            _movement.Move(movementAxis);
            
        }
    }
}
