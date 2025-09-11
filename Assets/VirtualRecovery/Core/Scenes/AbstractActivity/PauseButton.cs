// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 11/09/2025
//  */

using UnityEngine;
using UnityEngine.UI;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.AbstractActivity {
    internal class PauseButton : MonoBehaviour, IButton {
        [SerializeField] private ActivityCanvasChanger activityCanvasChanger;
        
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && activityCanvasChanger != null) {
                activityCanvasChanger.ChangeCanvas(new ActivityEventTypeWrapper(ActivityEventType.PauseTriggered));
            }
        }
    }
}