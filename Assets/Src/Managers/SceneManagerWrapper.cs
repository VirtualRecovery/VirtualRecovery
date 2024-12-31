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
        
        private const string k_mainMenuSceneName = "MainMenu";
        private const string k_kitchenSceneName = "Kitchen";

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadMainMenu() => SceneManager.LoadScene(k_mainMenuSceneName, LoadSceneMode.Single);
        public void LoadKitchen() => SceneManager.LoadScene(k_kitchenSceneName, LoadSceneMode.Single);
        
        void Start() {

        }

        void Update() {
        }
    }
}