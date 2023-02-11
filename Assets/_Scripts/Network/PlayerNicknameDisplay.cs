using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace InkeepersKeep.Core.Network
{
    public class PlayerNicknameDisplay : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _nicknameText;

        public override void OnNetworkSpawn() 
        { 
            NicknameRequestServerRpc(OwnerClientId);

            if (IsOwner)
                gameObject.SetActive(false);
        }

        [ServerRpc(RequireOwnership = false)]
        private void NicknameRequestServerRpc(ulong clientId)
        {
            PlayerData? playerData = NetworkClientData.GetPlayerData(clientId);

            if (playerData.HasValue)
                SendNicknameClientRpc((PlayerData)playerData);
        }

        [ClientRpc]
        private void SendNicknameClientRpc(PlayerData playerData)
        {
            if (_nicknameText)

            _nicknameText.text = playerData.PlayerName;
        } 

        private void LateUpdate() => transform.LookAt(transform.position - Camera.main.transform.forward);
    }
}
