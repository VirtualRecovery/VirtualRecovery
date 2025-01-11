// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 11/01/2025
//  */

using System.Collections.Generic;
using UnityEngine;

namespace VirtualRecovery.Core.UI {
    internal class CanvasChanger : MonoBehaviour {
        private Canvas m_currentCanvas;

        private readonly Stack<Canvas> m_previousCanvases = new Stack<Canvas>();
        
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas settingsCanvas;

        private Dictionary<UIEventType, Canvas> m_eventToCanvas;

        private void Awake() {
            m_currentCanvas = titleScreenCanvas;
            EnableCurrentCanvas();
            
            m_eventToCanvas = new Dictionary<UIEventType, Canvas> {
                { UIEventType.BeginSessionButtonClicked, sessionConfigurationCanvas },
                { UIEventType.PatientsListButtonClicked, patientsListCanvas },
                { UIEventType.SettingsButtonClicked, settingsCanvas },
                { UIEventType.BackToMainMenuButtonClicked, titleScreenCanvas }
            };
        }

        private void DisableCurrentCanvas() {
            m_currentCanvas.GetComponent<Canvas>().enabled = false;
        }
        
        private void EnableCurrentCanvas() {
            m_currentCanvas.GetComponent<Canvas>().enabled = false;
        }
        
        public void ChangeCanvas(UIEventType eventType) {
            DisableCurrentCanvas();
            if (eventType == UIEventType.ReturnButtonClicked) {
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
