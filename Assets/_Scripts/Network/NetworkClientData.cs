using System.Collections.Generic;

namespace InkeepersKeep.Core.Network
{
    public static class NetworkClientData
    {
        private static Dictionary<ulong, PlayerData> _clientData;

        public static void Initialize() => _clientData = new Dictionary<ulong, PlayerData>();

        public static void AddClient(ulong clientId, PlayerData playerData) => _clientData.Add(clientId, playerData);

        public static PlayerData? GetPlayerData(ulong clientId)
        {
            if (_clientData.TryGetValue(clientId, out PlayerData playerData))
                return playerData;

            return null;
        }

        public static bool IsPlayerDataExists(ulong clientId) => _clientData.ContainsKey(clientId);
    }
}
