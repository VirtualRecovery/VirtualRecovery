// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace VirtualRecovery.Core.Managers {
    internal class SceneManagerWrapper : MonoBehaviour {
        public static SceneManagerWrapper Instance { get; private set; }
        
        private string m_mainMenuSceneName;
        private string m_kitchenSceneName;
        private string m_livingRoomSceneName;
        private string m_bathroomSceneName;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this; DontDestroyOnLoad(gameObject);
            
            m_mainMenuSceneName = Configuration.Instance.configData.scenes.mainMenuSceneName;
            m_kitchenSceneName = Configuration.Instance.configData.scenes.kitchenSceneName;
            m_livingRoomSceneName = Configuration.Instance.configData.scenes.livingRoomSceneName;
            m_bathroomSceneName = Configuration.Instance.configData.scenes.bathroomSceneName;
        }

        public void LoadMainMenu() => SceneManager.LoadScene(m_mainMenuSceneName, LoadSceneMode.Single);
        public void LoadKitchen() => SceneManager.LoadScene(m_kitchenSceneName, LoadSceneMode.Single);
        public void LoadLivingRoom() => SceneManager.LoadScene(m_livingRoomSceneName, LoadSceneMode.Single);
        public void LoadBathroom() => SceneManager.LoadScene(m_bathroomSceneName, LoadSceneMode.Single);
        
        void Start() {

        }

        void Update() {
        }
    }
}