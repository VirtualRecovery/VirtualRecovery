// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 14/01/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.BaseClasses {
    internal abstract class BaseCanvasChanger : MonoBehaviour {
        protected Canvas CurrentCanvas;
        protected readonly Stack<Canvas> PreviousCanvases = new Stack<Canvas>();
        protected Dictionary<Enum, Canvas> EventToCanvas;

        internal void Initialize(Dictionary<Enum, Canvas> eventToCanvas, Canvas initialCanvas) {
            EventToCanvas = eventToCanvas; 
            CurrentCanvas = initialCanvas; 
            EnableCurrentCanvas();
        }
        
        protected virtual void EnableCurrentCanvas() {
            if (CurrentCanvas == null) return;
            CurrentCanvas.enabled = true;
        }

        protected virtual void DisableCurrentCanvas() {
            if (CurrentCanvas == null) return;
            CurrentCanvas.enabled = false;
        }

        internal abstract void ChangeCanvas(IEventTypeWrapper eventType);
    }
}