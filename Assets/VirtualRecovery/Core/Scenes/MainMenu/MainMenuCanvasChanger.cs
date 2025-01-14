﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal class MainMenuCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas titleScreenCanvas;
        [SerializeField] private Canvas patientsListCanvas;
        [SerializeField] private Canvas sessionConfigurationCanvas;
        [SerializeField] private Canvas settingsCanvas;
        [SerializeField] private Canvas patientSelectionCanvas;
        [SerializeField] private Canvas activitySelectionCanvas;

        private void Awake() {
            var eventToCanvas = new Dictionary<IEventTypeWrapper, Canvas> {
                { new MainMenuEventTypeWrapper(MainMenuEventType.BeginSessionButtonClicked), sessionConfigurationCanvas },
                { new MainMenuEventTypeWrapper(MainMenuEventType.PatientsListButtonClicked), patientsListCanvas },
                { new MainMenuEventTypeWrapper(MainMenuEventType.SettingsButtonClicked), settingsCanvas },
                { new MainMenuEventTypeWrapper(MainMenuEventType.BackToMainMenuButtonClicked), titleScreenCanvas },
                { new MainMenuEventTypeWrapper(MainMenuEventType.PatientSelectionButtonClicked), patientSelectionCanvas },
                { new MainMenuEventTypeWrapper(MainMenuEventType.ActivitySelectionButtonClicked), activitySelectionCanvas }
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
            if (eventType == MainMenuEventType.ReturnButtonClicked) {
                CurrentCanvas = PreviousCanvases.Pop();
            }
            else {
                PreviousCanvases.Push(CurrentCanvas);
                CurrentCanvas = EventToCanvas[eventTypeWrapper];
            }
            EnableCurrentCanvas();
        }
    }
}