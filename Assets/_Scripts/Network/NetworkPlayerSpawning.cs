using UnityEngine;
using Unity.Netcode;

namespace InkeepersKeep.Core.Network
{
    public class NetworkPlayerSpawning : MonoBehaviour
    {
        [SerializeField] private NetworkObject _playerPrefab;

        private void Awake()
        {
            foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
                SpawnPlayer(kvp.Key);
        }

        private void SpawnPlayer(ulong clientId)
        {
            var newPlayer = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
            newPlayer.SpawnWithOwnership(clientId, true);
        }
    }
}
