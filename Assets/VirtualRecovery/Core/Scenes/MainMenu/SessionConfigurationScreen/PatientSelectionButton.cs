﻿// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 14/01/2025
//  */

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu.SessionConfigurationScreen {
    internal class PatientSelectionButton : MonoBehaviour, IButton {
        [FormerlySerializedAs("mainMenuBaseCanvasChanger")] [SerializeField] private MainMenuCanvasChanger mainMenuCanvasChanger;

        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && mainMenuCanvasChanger != null) {
                mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                    MainMenuEventType.PatientSelectionButtonClicked)
                );
            }
        }
    }
}