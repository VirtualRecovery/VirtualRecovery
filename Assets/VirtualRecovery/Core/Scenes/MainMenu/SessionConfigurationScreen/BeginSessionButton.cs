// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 14/01/2025
//  */

using UnityEngine;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    internal class BeginSessionButton : MonoBehaviour, IButton {
        public void OnButtonClicked() {
            GameManager.Instance.BeginSession();
        }
    }
}