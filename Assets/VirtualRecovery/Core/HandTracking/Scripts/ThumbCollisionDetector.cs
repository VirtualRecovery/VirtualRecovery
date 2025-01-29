// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.HandTracking.Scripts {
                                                                               
    public class ThumbCollisionDetector : MonoBehaviour {
        
        [SerializeField] private HandGrabController m_handGrabController;
        
        private void OnTriggerEnter(Collider other) {
            m_handGrabController.HandleThumbTouch(this.gameObject, other.gameObject);
        }

        private void OnTriggerExit(Collider other) {
            m_handGrabController.HandleThumbRelease(this.gameObject, other.gameObject);
        }
        
    }
}