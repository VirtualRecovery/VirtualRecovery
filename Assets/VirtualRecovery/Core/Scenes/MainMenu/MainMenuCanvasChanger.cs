// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Scenes.BaseClasses;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal class MainMenuCanvasChanger : CanvasChangerBase<MainMenuEventType> {
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas settingsCanvas;
        [SerializeField] private Canvas patientSelectionCanvas;
        [SerializeField] private Canvas activitySelectionCanvas;

        private void Awake() {
            CurrentCanvas = titleScreenCanvas;
            EnableCurrentCanvas();
            
            EventToCanvas = new Dictionary<MainMenuEventType, Canvas> {
                { MainMenuEventType.BeginSessionButtonClicked, sessionConfigurationCanvas },
                { MainMenuEventType.PatientsListButtonClicked, patientsListCanvas },
                { MainMenuEventType.SettingsButtonClicked, settingsCanvas },
                { MainMenuEventType.BackToMainMenuButtonClicked, titleScreenCanvas },
                { MainMenuEventType.PatientSelectionButtonClicked, patientSelectionCanvas },
                { MainMenuEventType.ActivitySelectionButtonClicked, activitySelectionCanvas }
            };
        }
        
        internal override void ChangeCanvas(MainMenuEventType eventType) {
            if (eventType == MainMenuEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == MainMenuEventType.ReturnButtonClicked) {
                CurrentCanvas = PreviousCanvases.Pop();
            }
            else {
                PreviousCanvases.Push(CurrentCanvas);
                CurrentCanvas = EventToCanvas[eventType];
            }
            EnableCurrentCanvas();
        }
    }
}