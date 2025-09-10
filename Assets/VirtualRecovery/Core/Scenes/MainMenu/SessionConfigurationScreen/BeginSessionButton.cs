// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 14/01/2025
//  */

using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Scenes.Interfaces;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Scenes.MainMenu.SessionConfigurationScreen {
    internal class BeginSessionButton : MonoBehaviour, IButton {
        public void OnButtonClicked() {
            GetSessionConfig();

            GameManager.Instance.ClearSelectionFlags();
            GameManager.Instance.BeginSession();
        }
        
        private void GetSessionConfig() {
            var sessionConfig = transform.parent.transform.parent.gameObject.GetNamedChild("Config");
            if (sessionConfig == null) {
                Debug.LogError("SessionConfiguration not found in the parent hierarchy.");
                return;
            }
            
            var difficultyLevel = sessionConfig.GetNamedChild("Difficulty").GetNamedChild("Dropdown").GetComponent<TMPro.TMP_Dropdown>();
            var bodySide = sessionConfig.GetNamedChild("Bodyside").GetNamedChild("Dropdown").GetComponent<TMPro.TMP_Dropdown>();

            if (difficultyLevel == null || bodySide == null) {
                Debug.LogError("Difficulty or Bodyside dropdowns not found in the session configuration.");
                return;
            }
            
            GameManager.Instance.AddDifficulty((DifficultyLevel)difficultyLevel.value);
            GameManager.Instance.AddBodyside((BodySide)bodySide.value);
        }
    }
}