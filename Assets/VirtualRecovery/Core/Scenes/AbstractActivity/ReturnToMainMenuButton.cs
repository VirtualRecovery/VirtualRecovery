// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 13/08/2025
//  */

using UnityEngine;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.AbstractActivity {
    internal class ReturnToMainMenuButton : MonoBehaviour, IButton {
        public void OnButtonClicked() {
            GameManager.Instance.BackToMainMenu();
        }
    }
}