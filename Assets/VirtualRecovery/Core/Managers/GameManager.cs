// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using VirtualRecovery.Core.Scenes.MainMenu;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private SessionManager m_sessionManager;
        private SceneManagerWrapper m_sceneManagerWrapper;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_sessionManager = SessionManager.Instance;
            m_sceneManagerWrapper = SceneManagerWrapper.Instance;
        }

        private void Start() {
            m_sceneManagerWrapper.LoadMainMenu();
        }

        private void Update() { }
    }
}