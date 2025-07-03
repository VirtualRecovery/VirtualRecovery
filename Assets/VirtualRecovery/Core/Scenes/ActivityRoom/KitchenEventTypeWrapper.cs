// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 03/06/2025
//  */

using System;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.ActivityRoom {

    internal class ActivityRoomEventTypeWrapper : IEventTypeWrapper {
        public Enum EventType { get; set; }
        public ActivityRoomEventTypeWrapper(ActivityRoomEventType eventType) {
            EventType = eventType;
        }
    }
    
    internal enum ActivityRoomEventType {
        PauseButtonClicked,
        ResumeButtonClicked,
        SettingsButtonClicked,
        RestartButtonClicked,
        ExitButtonClicked
    }
}