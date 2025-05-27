// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 22/05/2025
//  */

using UnityEngine;
using UnityEngine.UI;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    internal class ChoosePatientButton : MonoBehaviour, IButton {
        private MainMenuCanvasChanger m_mainMenuCanvasChanger;

        private void Start() {
            m_mainMenuCanvasChanger = Object.FindFirstObjectByType<MainMenuCanvasChanger>();
            if (m_mainMenuCanvasChanger == null) {
                Debug.LogError("MainMenuCanvasChanger not found in the scene.");
            }
        }

        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && m_mainMenuCanvasChanger != null) {
                var patientData = GetComponentInParent<PatientData>();
                GameManager.Instance.SetPatient(patientData.patient);

                m_mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                    MainMenuEventType.PatientChosenButtonClicked));
            }
        }
    }
}
