// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 28/01/2025
//  */

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    internal class ConfirmPatientDeletionButton : MonoBehaviour, IButton {
        [FormerlySerializedAs("mainMenuBaseCanvasChanger")] [SerializeField] private MainMenuCanvasChanger mainMenuCanvasChanger;
        
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && mainMenuCanvasChanger != null) {
                Patient patient = GameManager.Instance.GetPatient();
                if (patient != null) {
                    PatientsRepository patientsRepository = new PatientsRepository();
                    patientsRepository.Delete(patient.Id);
                    GameManager.Instance.SetPatient(null);
                }
                
                mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                    MainMenuEventType.ConfirmPatientDeletionButton));
            }
        }
    }
}