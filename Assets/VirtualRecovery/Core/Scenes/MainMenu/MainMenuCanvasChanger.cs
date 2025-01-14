// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System.Collections.Generic;
using UnityEngine;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    public class MainMenuCanvasChanger : MonoBehaviour {
        private Canvas m_currentCanvas;

        private readonly Stack<Canvas> m_previousCanvases = new Stack<Canvas>();
        
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas settingsCanvas;
        [SerializeField] private Canvas patientSelectionCanvas;
        [SerializeField] private Canvas activitySelectionCanvas;

        private Dictionary<MainMenuEventType, Canvas> m_eventToCanvas;

        private void Awake() {
            m_currentCanvas = titleScreenCanvas;
            EnableCurrentCanvas();
            
            m_eventToCanvas = new Dictionary<MainMenuEventType, Canvas> {
                { MainMenuEventType.BeginSessionButtonClicked, sessionConfigurationCanvas },
                { MainMenuEventType.PatientsListButtonClicked, patientsListCanvas },
                { MainMenuEventType.SettingsButtonClicked, settingsCanvas },
                { MainMenuEventType.BackToMainMenuButtonClicked, titleScreenCanvas },
                { MainMenuEventType.PatientSelectionButtonClicked, patientSelectionCanvas },
                { MainMenuEventType.ActivitySelectionButtonClicked, activitySelectionCanvas }
            };
        }

        private void DisableCurrentCanvas() {
            m_currentCanvas.enabled = false;
        }
        
        private void EnableCurrentCanvas() {
            m_currentCanvas.enabled = true;
        }
        
        internal void ChangeCanvas(MainMenuEventType eventType) {
            if (eventType == MainMenuEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == MainMenuEventType.ReturnButtonClicked) {
                m_currentCanvas = m_previousCanvases.Pop();
            }
            else {
                m_previousCanvases.Push(m_currentCanvas);
                m_currentCanvas = m_eventToCanvas[eventType];
            }
            EnableCurrentCanvas();
        }
    }
}