// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 23/04/2025
//  */

using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scenes.MainMenu.DeletePatientScreen {
    internal class FillPatientData : MonoBehaviour {
        private bool m_wasCanvasEnabled;

        public void Update() {
            var canvas = GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                FillData();
            }
            m_wasCanvasEnabled = canvas.enabled;
        }

        private void FillData() {
            var patient = GameManager.Instance.GetPatient();
            
            if (patient == null) {
                Debug.LogError("Patient is null");
                return;
            }

            var patientData = this.gameObject.GetNamedChild("PatientData");
            var id = patientData.GetNamedChild("ID").GetNamedChild("DataDisplay").GetNamedChild("Text (TMP)").GetComponent<TextMeshProUGUI>();
            var icdCode = patientData.GetNamedChild("ICDCode").GetNamedChild("DataDisplay").GetNamedChild("Text (TMP)").GetComponent<TextMeshProUGUI>();
            var birthYear = patientData.GetNamedChild("BirthYear").GetNamedChild("DataDisplay").GetNamedChild("Text (TMP)").GetComponent<TextMeshProUGUI>();
            var gender = patientData.GetNamedChild("Gender").GetNamedChild("DataDisplay").GetNamedChild("Text (TMP)").GetComponent<TextMeshProUGUI>();

            id?.SetText(patient.Id.ToString());
            icdCode?.SetText(patient.IcdCode);
            birthYear?.SetText(patient.YearOfBirth.ToString());
            gender?.SetText(patient.Gender.ToString());
        }
    }
}