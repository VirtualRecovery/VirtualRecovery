// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace VirtualRecovery {
    internal class SceneManagerWrapper : MonoBehaviour {
        public static SceneManagerWrapper Instance { get; private set; }
        
        private string m_mainMenuSceneName;
        private string m_kitchenSceneName;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this; DontDestroyOnLoad(gameObject);
            
            if (Configuration.Instance != null && Configuration.Instance.configData != null) {
                m_mainMenuSceneName = Configuration.Instance.configData.scenes.mainMenuSceneName;
                m_kitchenSceneName = Configuration.Instance.configData.scenes.kitchenSceneName;
            } else {
                Debug.LogError("Configuration is not properly loaded.");
            }
        }

        public void LoadMainMenu() => SceneManager.LoadScene(m_mainMenuSceneName, LoadSceneMode.Single);
        public void LoadKitchen() => SceneManager.LoadScene(m_kitchenSceneName, LoadSceneMode.Single);
        
        void Start() {

        }

        void Update() {
        }
    }
}