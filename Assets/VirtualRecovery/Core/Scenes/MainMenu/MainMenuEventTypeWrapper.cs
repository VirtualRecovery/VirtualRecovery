// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using System;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu {

    internal class MainMenuEventTypeWrapper : IEventTypeWrapper {
        public Enum EventType { get; set; }
        public MainMenuEventTypeWrapper(MainMenuEventType eventType) {
            EventType = eventType;
        }
    }
    
    internal enum MainMenuEventType {
        ReturnButtonClicked,
        BeginSessionButtonClicked,
        PatientsListButtonClicked,
        SettingsButtonClicked,
        ExitButtonClicked,
        BackToMainMenuButtonClicked,
        PatientSelectionButtonClicked,
        ActivitySelectionButtonClicked,
        PatientChosenButtonClicked,
        ActivityChosenButtonClicked,
        AddPatientButtonClicked,
        EditPatientButtonClicked,
        DeletePatientButtonClicked,
        SavePatientDataButtonClicked,
        ConfirmPatientDeletionButton,
        ViewSessionsHistoryButtonClicked
    }
}