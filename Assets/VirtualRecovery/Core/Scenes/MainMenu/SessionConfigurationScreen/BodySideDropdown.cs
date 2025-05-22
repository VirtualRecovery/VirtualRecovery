// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 14/01/2025
//  */

using TMPro;
using UnityEngine;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Scenes.MainMenu.SessionConfigurationScreen {
    internal class BodySideDropdown : MonoBehaviour, IDropdown {
        public void OnValueChanged() {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown == null) {
                return;
            }

            var selectedValue = dropdown.value;
            var bodySide = (BodySide)selectedValue;
            GameManager.Instance.SetBodyside(bodySide);
        }
    }
}