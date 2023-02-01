using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InkeepersKeep.Core.UI
{
    public class NetworkManagerUI : MonoBehaviour
    {
        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _clientButton;
        [SerializeField] private TextMeshProUGUI _connectionIPAdress;
        [SerializeField] private TextMeshProUGUI _connectionPort;

        private void Awake()
        {
            _hostButton.onClick.AddListener(() =>
            {
                string ipAdress = _connectionIPAdress.text;
                string port = _connectionPort.text;
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipAdress.Remove(ipAdress.Length - 1, 1);
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(port.Remove(port.Length - 1, 1));

                NetworkManager.Singleton.StartHost();
            });

            _clientButton.onClick.AddListener(() =>
            {
                string ipAdress = _connectionIPAdress.text;
                string port = _connectionPort.text;
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipAdress.Remove(ipAdress.Length - 1, 1);
                NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(port.Remove(port.Length - 1, 1));

                NetworkManager.Singleton.StartClient();
            });
        }
    }
}
