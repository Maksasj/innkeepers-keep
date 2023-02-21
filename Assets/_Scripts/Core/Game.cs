using UnityEngine;

namespace InkeepersKeep.Core
{
    public class Game : MonoBehaviour
    {
        private const int INTERACTABLE_OBJECTS_LAYER = 6;
        private const int PLAYER_LAYER = 7;

        private void Awake()
        {
            Physics.IgnoreLayerCollision(INTERACTABLE_OBJECTS_LAYER, PLAYER_LAYER, true);
        }
    }
}
