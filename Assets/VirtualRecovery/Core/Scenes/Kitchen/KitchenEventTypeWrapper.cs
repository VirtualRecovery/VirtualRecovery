// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 03/06/2025
//  */

using System;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.Kitchen {

    internal class KitchenEventTypeWrapper : IEventTypeWrapper {
        public Enum EventType { get; set; }
        public KitchenEventTypeWrapper(KitchenEventType eventType) {
            EventType = eventType;
        }
    }
    
    internal enum KitchenEventType {
        PauseButtonClicked,
        ResumeButtonClicked,
        SettingsButtonClicked,
        RestartButtonClicked,
        ExitButtonClicked
    }
}