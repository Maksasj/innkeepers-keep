using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

namespace InkeepersKeep.Core.Network
{
    public class PlayerNicknameDisplay : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _nicknameText;

        private NetworkVariable<FixedString32Bytes> _nickname = new NetworkVariable<FixedString32Bytes>();

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;

            PlayerData? playerData = NetworkClientData.GetPlayerData(OwnerClientId);

            if (playerData.HasValue)
                _nickname.Value = playerData.Value.PlayerName;
        }

        private void OnEnable() => _nickname.OnValueChanged += DisplayNickname;
        private void OnDisable() => _nickname.OnValueChanged -= DisplayNickname;

        private void DisplayNickname(FixedString32Bytes oldDisplayName, FixedString32Bytes newDisplayName) => _nicknameText.text = newDisplayName.ConvertToString();
    }
}
