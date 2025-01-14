﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 12/01/2025
//  */

using UnityEngine;
using UnityEngine.UI;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu.TitleScreen {
    internal class SettingsButton : MonoBehaviour, IButton {
        [SerializeField] private MainMenuCanvasChanger mainMenuCanvasChanger;
        
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && mainMenuCanvasChanger != null) {
                mainMenuCanvasChanger.ChangeCanvas(MainMenuEventType.ReturnButtonClicked);
            }
        }
    }
}