// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 03/06/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.ActivityRoom {
    internal class ActivityRoomCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas therapistViewCanvas;
        [SerializeField] private Canvas pauseMenuCanvas;
        [SerializeField] private Canvas settingsCanvas;
        [SerializeField] private Canvas patientPauseCanvas;
        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { ActivityRoomEventType.PauseButtonClicked, pauseMenuCanvas },
                { ActivityRoomEventType.ResumeButtonClicked, therapistViewCanvas },
                { ActivityRoomEventType.SettingsButtonClicked, pauseMenuCanvas },
                { ActivityRoomEventType.RestartButtonClicked, therapistViewCanvas } // TODO: Add restart of excercise
            };
            
            Initialize(eventToCanvas, therapistViewCanvas);
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (ActivityRoomEventTypeWrapper)eventTypeWrapper;
            ActivityRoomEventType eventType = (ActivityRoomEventType)eventTypeWrapper.EventType;
            /*if (eventType == ActivityRoomEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == ActivityRoomEventType.BackToMainMenuButtonClicked) {
                CurrentCanvas = titleScreenCanvas;
                PreviousCanvases.Clear();
            }
            if (eventType is ActivityRoomEventType.ReturnButtonClicked 
                or ActivityRoomEventType.SavePatientDataButtonClicked 
                or ActivityRoomEventType.ConfirmPatientDeletionButton) {
                CurrentCanvas = PreviousCanvases.Pop();
            }
            else {
                PreviousCanvases.Push(CurrentCanvas);
                CurrentCanvas = EventToCanvas[eventType];
            }*/
            EnableCurrentCanvas();
        }
    }
}