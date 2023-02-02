using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core
{
    public class GameState : NetworkBehaviour
    {
        [SerializeField] private Camera _camera;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                Destroy(_camera.gameObject);

            SceneTransition.Singleton.SetSceneState(SceneTransition.SceneStates.Game);

            base.OnNetworkSpawn();
        }
    }
}
