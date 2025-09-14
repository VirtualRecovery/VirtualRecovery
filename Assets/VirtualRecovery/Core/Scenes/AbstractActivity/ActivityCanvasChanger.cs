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
        
        [SerializeField] private Camera playerCameraOverlay;
        [SerializeField] private Camera therapistCameraOverlay;

        private void Awake() {
            var eventToCanvas = new Dictionary<Enum, Canvas> {
                { ActivityEventType.SessionEnded, sessionEndCanvas },
                { ActivityEventType.ResumeButtonClicked, therapistViewCanvas },
                { ActivityEventType.RestartButtonClicked, therapistViewCanvas },
                { ActivityEventType.PauseTriggered, pauseMenuCanvas }
            };
            Initialize(eventToCanvas, therapistViewCanvas);
            playerCameraOverlay.enabled = false;
            therapistCameraOverlay.enabled = false;
        }
        
        internal override void ChangeCanvas(IEventTypeWrapper eventTypeWrapper) {
            eventTypeWrapper = (ActivityEventTypeWrapper)eventTypeWrapper;
            ActivityEventType eventType = (ActivityEventType)eventTypeWrapper.EventType;
            DisableCurrentCanvas();
            
            if (eventType == ActivityEventType.BackToMainMenuButtonClicked) {
                GameManager.Instance.BackToMainMenu();
                playerCameraOverlay.enabled = true;
                therapistCameraOverlay.enabled = true;
            }
            if (eventType is ActivityEventType.ResumeButtonClicked) {
                GameManager.Instance.ResumeGame();
                DisableCurrentCanvas();
                playerCameraOverlay.enabled = false;
                therapistCameraOverlay.enabled = false;
            }
            if (eventType is ActivityEventType.PauseTriggered) {
                if (CurrentCanvas == sessionEndCanvas) return;
                playerCameraOverlay.enabled = true;
                therapistCameraOverlay.enabled = true;
                GameManager.Instance.PauseGame();
            }

            if (eventType is ActivityEventType.SessionEnded) {
                playerCameraOverlay.enabled = true;
                therapistCameraOverlay.enabled = true;
            }
            
            if (eventType is ActivityEventType.RestartButtonClicked) {
                GameManager.Instance.RestartActivity();
                playerCameraOverlay.enabled = true;
                therapistCameraOverlay.enabled = true;
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