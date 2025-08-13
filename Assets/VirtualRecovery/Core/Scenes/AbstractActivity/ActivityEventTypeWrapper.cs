// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.AbstractActivity {

    internal class ActivityEventTypeWrapper : IEventTypeWrapper {
        public Enum EventType { get; set; }
        public ActivityEventTypeWrapper(ActivityEventType eventType) {
            EventType = eventType;
        }
    }
    
    internal enum ActivityEventType {
        SessionEnded,
        ExitButtonClicked,
        BackToMainMenuButtonClicked,
        ReturnButtonClicked,
    }
}