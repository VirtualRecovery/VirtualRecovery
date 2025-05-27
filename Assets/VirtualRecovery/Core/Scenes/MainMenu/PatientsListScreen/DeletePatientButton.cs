// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 28/01/2025
//  */

using UnityEngine;
using UnityEngine.UI;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.Core.Scenes.MainMenu.Common;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Scenes.MainMenu.PatientsListScreen {
    internal class DeletePatientButton : MonoBehaviour, IButton {
        private MainMenuCanvasChanger m_mainMenuCanvasChanger;
        
        private void Start() {
            m_mainMenuCanvasChanger = Object.FindFirstObjectByType<MainMenuCanvasChanger>();
            if (m_mainMenuCanvasChanger == null) {
                Debug.LogError("MainMenuCanvasChanger not found in the scene.");
            }
        }
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            Patient patient = button.gameObject.GetComponentInParent<PatientData>().patient;
            if (patient != null) {
                GameManager.Instance.SetPatient(patient);
            }
            if (button != null && m_mainMenuCanvasChanger != null) {
                m_mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                    MainMenuEventType.DeletePatientButtonClicked));
            }
        }
    }
}