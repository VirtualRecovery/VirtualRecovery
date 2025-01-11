// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using VirtualRecovery.Core.UI;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal class TitleScreen : MonoBehaviour {
        private CanvasChanger m_canvasChanger;

        private void Awake() {
            m_canvasChanger = FindFirstObjectByType<CanvasChanger>();
        }

        public void GoToSessionConfiguration() {
            m_canvasChanger.ChangeCanvas(UIEventType.BeginSessionButtonClicked);
        }
        
        public void GoToPatientsList() {
            m_canvasChanger.ChangeCanvas(UIEventType.PatientsListButtonClicked);
        }

        public void GoToSettings() {
            m_canvasChanger.ChangeCanvas(UIEventType.SettingsButtonClicked);
        }

        public void QuitApp() {
            Application.Quit();
        }
    }
}