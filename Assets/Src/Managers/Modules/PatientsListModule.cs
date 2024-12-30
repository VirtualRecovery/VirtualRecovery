// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery {
    internal class PatientsListModule : MonoBehaviour {
        internal static PatientsListModule Instance { get; private set; }
        private PatientRepository m_patientRepository = new PatientRepository();

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start() {

        }

        void Update() {
        }
    }
}