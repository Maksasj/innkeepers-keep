using InkeepersKeep.Core.Entities;
using UnityEngine;

namespace InkeepersKeep.Core
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private Player _player;

        private void Start()
        {
            _input.Initialize();
            _input.Enable();

            _player.Initialize();
        }
    }
}
