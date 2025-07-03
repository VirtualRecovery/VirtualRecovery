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
using VirtualRecovery.Core.Scenes.Kitchen;

namespace VirtualRecovery.Core.Scenes.Kitchen {
    internal class KitchenCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas therapistViewCanvas;
        [SerializeField] private Canvas pauseMenuCanvas;
        [SerializeField] private Canvas settingsCanvas;
        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { KitchenEventType.PauseButtonClicked, pauseMenuCanvas },
                { KitchenEventType.ResumeButtonClicked, therapistViewCanvas },
                { KitchenEventType.SettingsButtonClicked, pauseMenuCanvas},
                { KitchenEventType.RestartButtonClicked, therapistViewCanvas }, // TODO: Add restart of excercise
            };
            
            Initialize(eventToCanvas, therapistViewCanvas);
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (KitchenEventTypeWrapper)eventTypeWrapper;
            KitchenEventType eventType = (KitchenEventType)eventTypeWrapper.EventType;
            /*if (eventType == KitchenEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == KitchenEventType.BackToMainMenuButtonClicked) {
                CurrentCanvas = titleScreenCanvas;
                PreviousCanvases.Clear();
            }
            if (eventType is KitchenEventType.ReturnButtonClicked 
                or KitchenEventType.SavePatientDataButtonClicked 
                or KitchenEventType.ConfirmPatientDeletionButton) {
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