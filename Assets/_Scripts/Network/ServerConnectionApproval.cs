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
            Debug.Log("Approval Check");

            string payload = Encoding.ASCII.GetString(request.Payload);
            var connectionPayload = JsonUtility.FromJson<ConnectionPayload>(payload);

            response.Approved = connectionPayload.playerName.Length > 0;

            Debug.Log("Player nickname: " + connectionPayload.playerName);
            Debug.Log("Connection allowed: " + response.Approved);

            if (response.Approved)
            {
                response.CreatePlayerObject = true;

                if (!NetworkClientData.IsPlayerDataExists(request.ClientNetworkId))
                {
                    Debug.Log("Adding player data");

                    PlayerData playerData = new PlayerData(connectionPayload.playerName);
                    NetworkClientData.AddClient(request.ClientNetworkId, playerData);
                }
            }

            response.Pending = false;
        }
    }
}
