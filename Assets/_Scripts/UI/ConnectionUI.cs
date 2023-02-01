using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Threading.Tasks;

namespace InkeepersKeep.Core.UI
{
    public class ConnectionUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _ipInputField;
        [SerializeField] private TMP_InputField _portInputField;

        private UnityTransport _unityTransport;

        private bool _isHost;

        public void Host() => _isHost = true;
        public void Client() => _isHost = false;

        public void Connect()
        {
            _unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();

            _unityTransport.ConnectionData.Address = _ipInputField.text;
            _unityTransport.ConnectionData.Port = ushort.Parse(_portInputField.text);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            if (_isHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();
        }
    }
}
