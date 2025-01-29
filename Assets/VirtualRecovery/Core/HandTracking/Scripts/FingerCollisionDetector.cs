// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 29/01/2025
//  */

using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class FingerCollisionDetector : MonoBehaviour {
        [SerializeField] private HandGrabController m_handGrabController;

        private void OnTriggerEnter(Collider other) {
            m_handGrabController.HandleFingerTouch(other.gameObject);
            
        }

        private void OnTriggerExit(Collider other) {
            m_handGrabController.HandleFingerRelease(other.gameObject);
        }
        
    }
}