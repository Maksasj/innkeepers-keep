using UnityEngine;

namespace InkeepersKeep.Core.Entities
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private float _speed;

        public void Move(Vector2 direction)
        {
            Vector3 movement = (direction.y * transform.forward) + (direction.x * transform.right);
            _controller.Move(movement * _speed * Time.deltaTime);
        }
    }
}
