// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System.Collections.Generic;
using UnityEngine;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    public class MainMenuCanvasManager : MonoBehaviour {
        private Canvas m_currentCanvas;

        private readonly Stack<Canvas> m_previousCanvases = new Stack<Canvas>();
        
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas settingsCanvas;

        private Dictionary<MainMenuEvents, Canvas> m_eventToCanvas;

        private void Awake() {
            m_currentCanvas = titleScreenCanvas;
            EnableCurrentCanvas();
            
            m_eventToCanvas = new Dictionary<MainMenuEvents, Canvas> {
                { MainMenuEvents.BeginSessionButtonClicked, sessionConfigurationCanvas },
                { MainMenuEvents.PatientsListButtonClicked, patientsListCanvas },
                { MainMenuEvents.SettingsButtonClicked, settingsCanvas },
                { MainMenuEvents.BackToMainMenuButtonClicked, titleScreenCanvas }
            };
        }

        private void DisableCurrentCanvas() {
            m_currentCanvas.GetComponent<Canvas>().enabled = false;
        }
        
        private void EnableCurrentCanvas() {
            m_currentCanvas.GetComponent<Canvas>().enabled = false;
        }
        
        internal void ChangeCanvas(MainMenuEvents eventType) {
            DisableCurrentCanvas();
            if (eventType == MainMenuEvents.ReturnButtonClicked) {
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