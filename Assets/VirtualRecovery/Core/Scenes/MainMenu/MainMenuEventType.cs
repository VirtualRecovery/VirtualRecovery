// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu {
    internal enum MainMenuEventType {
        ReturnButtonClicked,
        BeginSessionButtonClicked,
        PatientsListButtonClicked,
        SettingsButtonClicked,
        ExitButtonClicked,
        BackToMainMenuButtonClicked,
        PatientSelectionButtonClicked,
        ActivitySelectionButtonClicked
    }
}