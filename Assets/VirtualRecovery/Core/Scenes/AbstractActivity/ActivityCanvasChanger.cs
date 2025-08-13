// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 13/08/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.AbstractActivity {
    internal class ActivityCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas sessionEndCanvas;

        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { ActivityEventType.SessionEnded, sessionEndCanvas },
            };
            Initialize(eventToCanvas, null);
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (ActivityEventTypeWrapper)eventTypeWrapper;
            ActivityEventType eventType = (ActivityEventType)eventTypeWrapper.EventType;
            if (eventType == ActivityEventType.ExitButtonClicked) {
                Application.Quit();
            }
            
            DisableCurrentCanvas();
            if (eventType == ActivityEventType.BackToMainMenuButtonClicked) {
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            }
            if (eventType is ActivityEventType.ReturnButtonClicked) {
                CurrentCanvas = PreviousCanvases.Pop();
            }
            else {
                if (CurrentCanvas == null) {
                    CurrentCanvas = EventToCanvas[eventType];
                }
                PreviousCanvases.Push(CurrentCanvas);
            }
            EnableCurrentCanvas();
        }
    }
}