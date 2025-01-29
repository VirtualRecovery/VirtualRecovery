// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using System;
using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scripts {
    
    
    public class ThumbCollisionDetector : MonoBehaviour {
        public GameObject handController;
        private HandGrabController m_handGrabController;
        
        private void Awake() {
            m_handGrabController = handController.GetComponent<HandGrabController>();
        }
        private void OnTriggerEnter(Collider other) {
            m_handGrabController.HandleThumbTouch(this.gameObject, other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
            m_handGrabController.HandleThumbRelease(this.gameObject, other.gameObject);
        }
        
    }
}