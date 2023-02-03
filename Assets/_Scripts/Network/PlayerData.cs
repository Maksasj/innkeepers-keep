namespace InkeepersKeep.Core.Network
{
    public struct PlayerData
    {
        private string _playerName;

        public string PlayerName => _playerName;

        public PlayerData(string plyaerName) => _playerName = plyaerName;
    }
}
