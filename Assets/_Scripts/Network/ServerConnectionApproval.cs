using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace InkeepersKeep.Core.Network
{
    public class ServerConnectionApproval : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            string payload = Encoding.ASCII.GetString(request.Payload);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            response.Approved = connectionPayload.playerName.Length > 0;

            if (response.Approved)
            {
                response.CreatePlayerObject = true;

                if (!NetworkClientData.IsPlayerDataExists(request.ClientNetworkId))
                {
                    PlayerData playerData = new PlayerData(connectionPayload.playerName);
                    NetworkClientData.AddClient(request.ClientNetworkId, playerData);
                }
            }

            response.Pending = false;
        }
    }
}
