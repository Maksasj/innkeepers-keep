using InkeepersKeep.Core.Entities.Player;
using UnityEngine;

namespace InkeepersKeep.Core
{
    public class Game : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
