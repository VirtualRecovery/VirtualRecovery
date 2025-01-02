// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Modules {
    internal class MainMenuModule : MonoBehaviour {
        private PatientsListModule m_patientsListModule;
        private SessionConfigurationModule m_sessionConfigurationModule;

        private void Awake() {
            m_patientsListModule = FindFirstObjectByType<PatientsListModule>();
            m_sessionConfigurationModule = FindFirstObjectByType<SessionConfigurationModule>();
        }

        void Start() { }

        void Update() { }
    }
}