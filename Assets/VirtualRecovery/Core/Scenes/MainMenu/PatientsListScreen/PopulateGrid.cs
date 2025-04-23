// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 23/04/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Scenes.MainMenu.Common;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.PatientsListScreen {
    internal class PopulateGrid : MonoBehaviour {
        public GameObject prefab;
        private PatientsRepository m_patientsRepository;
        private bool m_wasCanvasEnabled;
        
        private void Update() {
            var canvas = transform.parent.transform.parent.transform.parent.GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                m_patientsRepository = new PatientsRepository();
                var patients = m_patientsRepository.GetAll();
                ClearPatients();
                Populate(patients);
            }
            m_wasCanvasEnabled = canvas.enabled;
        }
        
        private void ClearPatients() {
            foreach (Transform child in transform) {
                if (child.name != "PlaceholderPatientRecord") {
                    Destroy(child.gameObject);
                }
            }
        }

        private void Populate(List<Patient> patients) {
            GameObject newGameObject;

            foreach (var patient in patients) {
                newGameObject = (GameObject)Instantiate(prefab, transform);
                FillPatientData(newGameObject, patient);
            }
        }

        private void FillPatientData(GameObject newGameObject, Patient patient) {
            newGameObject.GetComponent<PatientData>().patient = patient;
            
            var id = newGameObject.GetNamedChild("ID").GetNamedChild("Text");
            var icdCode = newGameObject.GetNamedChild("ICDCode").GetNamedChild("Text");
            var yearOfBirth = newGameObject.GetNamedChild("YearOfBirth").GetNamedChild("Text");
            var gender = newGameObject.GetNamedChild("Gender").GetNamedChild("Text");
            var weakBodySide = newGameObject.GetNamedChild("WeakBodySide").GetNamedChild("Text");
            
            if (id != null) {
                id.GetComponent<TextMeshProUGUI>().text = patient.Id.ToString();
            }
            
            if (icdCode != null) {
                icdCode.GetComponent<TextMeshProUGUI>().text = patient.IcdCode;
            }
            
            if (yearOfBirth != null) {
                yearOfBirth.GetComponent<TextMeshProUGUI>().text = patient.YearOfBirth.ToString();
            }

            if (gender != null) {
                gender.GetComponent<TextMeshProUGUI>().text = patient.Gender.ToString();
            }
            
            if (weakBodySide != null) {
                weakBodySide.GetComponent<TextMeshProUGUI>().text = patient.WeakBodySide.ToString();
            }
        }
    }
}