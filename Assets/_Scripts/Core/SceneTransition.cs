using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace InkeepersKeep.Core
{
    public class SceneTransition : MonoBehaviour
    {
        static public SceneTransition Singleton { get; private set; }
        
        public Action<ulong> OnClientLoadedScene;
        public Action<SceneStates> OnSceneStateChanged;

        private SceneStates _sceneState;
        public SceneStates SceneState => _sceneState;

        private const int MAIN_MENU_SCENE_BUILD_INDEX = 1;

        public enum SceneStates
        {
            Init,
            MainMenu,
            Game
        }

        private void Awake()
        {
            if (Singleton != this && Singleton != null)
                Destroy(Singleton.gameObject);

            Singleton = this;

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (_sceneState == SceneStates.Init)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void SetSceneState(SceneStates sceneState)
        {
            _sceneState = sceneState;
            OnSceneStateChanged?.Invoke(sceneState);
        }

        public void SwitchScene(string sceneName)
        {
            if (NetworkManager.Singleton.IsListening)
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            else
                SceneManager.LoadSceneAsync(sceneName);
        }

        public void RegisterCallbacks() => NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;

        private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            OnClientLoadedScene?.Invoke(clientId);
        }

        public void LoadMainMenu()
        {
            NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
            OnClientLoadedScene = null;

            SetSceneState(SceneStates.MainMenu);
            SceneManager.LoadScene(MAIN_MENU_SCENE_BUILD_INDEX);
        }
    }
}
