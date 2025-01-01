// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery {
    internal class PatientsListModule : MonoBehaviour {
        private PatientRepository m_patientRepository;

        private void Awake() {
            m_patientRepository = new PatientRepository();
        }

        void Start() { }

        void Update() { }
    }
}