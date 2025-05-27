// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 26/01/2025
//  */

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.Interfaces;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    internal class ChooseActivityButton : MonoBehaviour, IButton {
        private MainMenuCanvasChanger m_mainMenuCanvasChanger;
        
        private void Start() {
            m_mainMenuCanvasChanger = Object.FindFirstObjectByType<MainMenuCanvasChanger>();
            if (m_mainMenuCanvasChanger == null) {
                Debug.LogError("MainMenuCanvasChanger not found in the scene.");
            }
        }
        
        public void OnButtonClicked() {
            var button = GetComponent<Button>();
            if (button != null && m_mainMenuCanvasChanger != null) {
                var activityData = GetComponentInParent<ActivityData>();
                GameManager.Instance.SetActivity(activityData.activity);
                GameManager.Instance.SetRoom(activityData.room);
                
                m_mainMenuCanvasChanger.ChangeCanvas(new MainMenuEventTypeWrapper(
                    MainMenuEventType.ActivityChosenButtonClicked));
            }
        }
    }
}