﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal class PatientsList : MonoBehaviour {
        private PatientsRepository m_patientsRepository;

        private void Awake() {
            m_patientsRepository = new PatientsRepository();
        }

        void Start() { }

        void Update() { }
    }
}