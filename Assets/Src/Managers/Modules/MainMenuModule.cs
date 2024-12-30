// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery {
    internal class MainMenuModule : MonoBehaviour {
        internal static MainMenuModule Instance { get; private set; }
        private PatientsListModule m_patientsListModule;
        private SessionConfigurationModule m_sessionConfigurationModule;
        
        // TODO: Handle the scope of singleton life (between scenes)
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            m_patientsListModule = PatientsListModule.Instance;
            m_sessionConfigurationModule = SessionConfigurationModule.Instance;
        }

        void Start() {

        }

        void Update() {
        }
    }
}