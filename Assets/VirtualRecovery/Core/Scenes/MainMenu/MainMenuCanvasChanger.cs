// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal class MainMenuCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas patientSelectionCanvas;
        [SerializeField] private Canvas activitySelectionCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionsHistoryCanvas;
        [SerializeField] private Canvas settingsCanvas;
        [SerializeField] private Canvas addPatientCanvas;
        [SerializeField] private Canvas editPatientCanvas;
        [SerializeField] private Canvas deletePatientCanvas;
        [SerializeField] private Canvas pauseMenuCanvas;
        [SerializeField] private Canvas excerciseEndCanvas;
        [SerializeField] private Canvas therapistViewCanvas;
        [SerializeField] private Canvas excerciseHintCanvas;

        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { MainMenuEventType.BeginSessionButtonClicked, sessionConfigurationCanvas },
                { MainMenuEventType.PatientsListButtonClicked, patientsListCanvas },
                { MainMenuEventType.SettingsButtonClicked, settingsCanvas },
                { MainMenuEventType.PatientSelectionButtonClicked, patientSelectionCanvas },
                { MainMenuEventType.ActivitySelectionButtonClicked, activitySelectionCanvas },
                { MainMenuEventType.PatientChosenButtonClicked, sessionConfigurationCanvas},
                { MainMenuEventType.ActivityChosenButtonClicked, patientSelectionCanvas},
                { MainMenuEventType.AddPatientButtonClicked, addPatientCanvas},
                { MainMenuEventType.EditPatientButtonClicked, editPatientCanvas},
                { MainMenuEventType.DeletePatientButtonClicked, deletePatientCanvas},
                { MainMenuEventType.ViewSessionsHistoryButtonClicked, sessionsHistoryCanvas }
            };
            
            Initialize(eventToCanvas, titleScreenCanvas);
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (MainMenuEventTypeWrapper)eventTypeWrapper;
            MainMenuEventType eventType = (MainMenuEventType)eventTypeWrapper.EventType;
            if (eventType == MainMenuEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == MainMenuEventType.BackToMainMenuButtonClicked) {
                CurrentCanvas = titleScreenCanvas;
                PreviousCanvases.Clear();
            }
            if (eventType is MainMenuEventType.ReturnButtonClicked 
                or MainMenuEventType.SavePatientDataButtonClicked 
                or MainMenuEventType.ConfirmPatientDeletionButton) {
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