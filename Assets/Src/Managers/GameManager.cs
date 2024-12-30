// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery {
    internal class GameManager : MonoBehaviour {
        internal static GameManager Instance { get; private set; }

        private MainMenuModule m_mainMenuManager;
        private SessionManager m_sessionManager;
        private SceneManagerWrapper m_sceneManagerWrapper;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_mainMenuManager = MainMenuModule.Instance;
            m_sessionManager = SessionManager.Instance;
            m_sceneManagerWrapper = SceneManagerWrapper.Instance;
        }

        private void Start() {
            m_sceneManagerWrapper.LoadMainMenu();
        }

        private void Update() {
        }
    }
}