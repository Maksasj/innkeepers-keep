using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.SceneManagement;
using InkeepersKeep.Core.Network;
using System.Text;
using System;

namespace InkeepersKeep.Core
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private ServerConnectionApproval _serverConnectionApproval;

        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private TMP_InputField _portInputField;
        [SerializeField] private TMP_InputField _nicknameInputField;

        private UnityTransport _unityTransport;

        private bool _isHost;

        public void Host() => _isHost = true;
        public void Client() => _isHost = false;

        public void TryConnect()
        {
            if (!NicknameIsValid())
            {
                Debug.LogError("Nickname field cannot be empty!");
                return;
            }
                
            _unityTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            _unityTransport.ConnectionData.Address = _ipInputField.text;
            _unityTransport.ConnectionData.Port = ushort.Parse(_portInputField.text);

            if (_isHost)
            {
                PlayerData newPlayerData = new PlayerData(_nicknameInputField.text);

                NetworkClientData.Initialize();
                NetworkClientData.AddClient(NetworkManager.Singleton.LocalClientId, newPlayerData);

                var serverConnectionApprovalInstance = Instantiate(_serverConnectionApproval, transform.position, Quaternion.identity);
                NetworkManager.Singleton.ConnectionApprovalCallback += serverConnectionApprovalInstance.ApprovalCheck;

                TransferData(_nicknameInputField.text);

                if (NetworkManager.Singleton.StartHost())
                {
                    SceneTransition.Singleton.RegisterCallbacks();

                    string nextScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
                    SceneTransition.Singleton.SwitchScene(nextScenePath);
                }
            }
            else
            {
                TransferData(_nicknameInputField.text);

                if (!NetworkManager.Singleton.StartClient())
                {
                    Debug.LogError("Failed to start client.");
                    NetworkManager.Singleton.Shutdown();
                }
            }
        }

        private bool NicknameIsValid()
        {
            return _nicknameInputField.text.Length > 0;
        }

        private void TransferData(string nickname)
        {
            var payload = JsonUtility.ToJson(new ConnectionPayload()
            {
                playerName = nickname
            });

            byte[] payloadBytes = Encoding.ASCII.GetBytes(payload);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        }
    }
}
