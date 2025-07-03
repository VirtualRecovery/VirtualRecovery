// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 02/07/2025
//  */

using UnityEngine;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.Core.Scenes.ActivityRoom;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class PatientGesturesDetection : MonoBehaviour {

        Canvas m_pauseCanvas;
        
        private void Start() {
            m_pauseCanvas =  GameObject.FindWithTag("PlayerPauseCanvas").GetComponent<Canvas>();
        }
        
        public void OnThumbsUpPoseDetection() {
            Debug.Log("Thumbs Up Pose Detected");
            if (!GameManager.Instance.IsGamePaused()) {
                GameManager.Instance.PauseGame();
                m_pauseCanvas.enabled = true;
            }
            else {
                GameManager.Instance.ResumeGame();
                m_pauseCanvas.enabled = false;
            }
        }
    }
}