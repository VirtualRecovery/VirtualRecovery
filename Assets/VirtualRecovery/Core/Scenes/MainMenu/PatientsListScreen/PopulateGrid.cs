// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 23/04/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.PatientsListScreen {
    internal class PopulateGrid : MonoBehaviour {
        public GameObject prefab;
        private PatientsRepository m_patientsRepository;

        private void OnEnable() {
            m_patientsRepository = new PatientsRepository();
            var patients = m_patientsRepository.GetAll();
            for (int i = 0; i < 20; i++) {
                Patient patient = new Patient();
                patient.Id = i;
                patient.IcdCode = "ICD-10";
                patient.YearOfBirth = 1990;
                patient.Gender = Gender.Mężczyzna;
                patient.WeakBodySide = BodySide.Lewa;
                patients.Add(patient);
            }
            Populate(patients);
        }

        private void Populate(List<Patient> patients) {
            GameObject newGameObject;

            foreach (var patient in patients) {
                newGameObject = (GameObject)Instantiate(prefab, transform);
                FillPatientData(newGameObject, patient);
            }
        }

        private void FillPatientData(GameObject newGameObject, Patient patient) {
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