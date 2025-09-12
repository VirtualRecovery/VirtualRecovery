// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 13/08/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.AbstractActivity {
    internal class ActivityCanvasChanger : BaseCanvasChanger {
        [SerializeField] private Canvas sessionEndCanvas;
        [SerializeField] private Canvas therapistViewCanvas;
        [SerializeField] private Canvas pauseMenuCanvas;

        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { ActivityEventType.SessionEnded, sessionEndCanvas },
                { ActivityEventType.ResumeButtonClicked, therapistViewCanvas },
                { ActivityEventType.RestartButtonClicked, therapistViewCanvas },
                { ActivityEventType.PauseTriggered, pauseMenuCanvas }
            };
            Initialize(eventToCanvas, therapistViewCanvas);
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (ActivityEventTypeWrapper)eventTypeWrapper;
            ActivityEventType eventType = (ActivityEventType)eventTypeWrapper.EventType;
            DisableCurrentCanvas();
            
            if (eventType == ActivityEventType.BackToMainMenuButtonClicked) {
                GameManager.Instance.BackToMainMenu();
            }
            if (eventType is ActivityEventType.ResumeButtonClicked) {
                GameManager.Instance.ResumeGame();
                DisableCurrentCanvas();
            }
            if (eventType is ActivityEventType.PauseTriggered) {
                if (CurrentCanvas == sessionEndCanvas) return;
                GameManager.Instance.PauseGame();
            }
            
            if (eventType is ActivityEventType.RestartButtonClicked) {
                GameManager.Instance.RestartActivity();
            }
            else {
                PreviousCanvases.Push(CurrentCanvas);
                CurrentCanvas = EventToCanvas[eventType];
            }
            
            if (CurrentCanvas != null && CurrentCanvas.transform.parent != null) {
                var faceXRCamera = CurrentCanvas.transform.parent.GetComponent<FaceXRCamera>();
                if (faceXRCamera != null) {
                    faceXRCamera.RepositionNow();
                }
            }
            EnableCurrentCanvas();
        }
    }
}