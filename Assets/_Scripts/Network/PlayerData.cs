using Unity.Netcode;

namespace InkeepersKeep.Core.Network
{
    public struct PlayerData : INetworkSerializable
    {
        private string _playerName;

        public string PlayerName => _playerName;

        public PlayerData(string plyaerName) => _playerName = plyaerName;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _playerName);
        }
    }
}
