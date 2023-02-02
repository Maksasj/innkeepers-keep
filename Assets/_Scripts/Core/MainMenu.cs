using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.SceneManagement;

namespace InkeepersKeep.Core
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private TMP_InputField _portInputField;

        private UnityTransport _unityTransport;

        private bool _isHost;

        public void Host() => _isHost = true;
        public void Client() => _isHost = false;

        public void Connect()
        {
            _unityTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;

            _unityTransport.ConnectionData.Address = _ipInputField.text;
            _unityTransport.ConnectionData.Port = ushort.Parse(_portInputField.text);

            if (_isHost)
            {
                if (NetworkManager.Singleton.StartHost())
                {
                    SceneTransition.Singleton.RegisterCallbacks();

                    string nextScenePath = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1);
                    SceneTransition.Singleton.SwitchScene(nextScenePath);
                }
            }
            else
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
