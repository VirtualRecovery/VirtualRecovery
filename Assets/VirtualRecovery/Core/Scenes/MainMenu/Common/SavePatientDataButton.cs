// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 28/01/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    internal class SavePatientDataButton : MonoBehaviour, IButton {
        [FormerlySerializedAs("mainMenuBaseCanvasChanger")] [SerializeField] private MainMenuCanvasChanger mainMenuCanvasChanger;
        
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && mainMenuCanvasChanger != null) {
                if (HandlePatientCreation()) {
                    mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                        MainMenuEventType.SavePatientDataButtonClicked));
                }
            }
        }

        private bool HandlePatientCreation() {
            var patientData = transform.parent.transform.parent.gameObject.GetNamedChild("PatientData");
            var icdCode = patientData.GetNamedChild("ICDCodeInput").GetNamedChild("InputField (TMP)").GetComponent<TMP_InputField>().text;
            var yearOfBirth = patientData.GetNamedChild("BirthYearInput").GetNamedChild("InputField (TMP)").GetComponent<TMP_InputField>().text;
            var gender = patientData.GetNamedChild("GenderInput").GetNamedChild("GenderDropdown").GetComponent<TMP_Dropdown>().value;
            var weakBodySide = patientData.GetNamedChild("WeakBodySideInput").GetNamedChild("WeakBodySideDropdown").GetComponent<TMP_Dropdown>().value;
            
            if (string.IsNullOrEmpty(icdCode) || string.IsNullOrEmpty(yearOfBirth)) {
                Debug.LogError("ICD Code or Year of Birth is empty.");
                return false;
            }

            if (int.TryParse(yearOfBirth, out int year)) {
                var patientRepository = new PatientsRepository();
                var patient = new Patient {
                    IcdCode = icdCode,
                    YearOfBirth = year,
                    Gender = (Gender)gender,
                    WeakBodySide = (BodySide)weakBodySide
                };
                patientRepository.Insert(patient);
                return true;
            }

            return false;
        }
    }
}