// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 14/01/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualRecovery.Core.BaseClasses {
    internal abstract class CanvasChangerBase<T> : MonoBehaviour where T : Enum {
        protected Canvas CurrentCanvas;
        protected readonly Stack<Canvas> PreviousCanvases = new Stack<Canvas>();
        protected Dictionary<T, Canvas> EventToCanvas;

        protected virtual void EnableCurrentCanvas() {
            CurrentCanvas.enabled = true;
        }

        protected virtual void DisableCurrentCanvas() {
            CurrentCanvas.enabled = false;
        }

        internal abstract void ChangeCanvas(T eventType);
    }
}