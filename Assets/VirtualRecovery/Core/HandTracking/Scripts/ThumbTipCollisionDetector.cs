// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VirtualRecovery.Core.HandTracking.Scripts {
                                                                               
    public class ThumbTipCollisionDetector : MonoBehaviour {
        
        [SerializeField] private HandGrabController mHandGrabController;
        private String m_grabbableTag = "Grabbable";
        
        private void OnCollisionEnter(Collision collision) {
//            Debug.Log("Thumb collision enter:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                mHandGrabController.ThumbTipCollisionEnter(this.gameObject, collision.gameObject, collision);    
            }
        }

        private void OnCollisionExit(Collision collision) { 
            // Debug.Log("Thumb collision exit:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                mHandGrabController.ThumbTipCollisionExit(this.gameObject);
            }
        }
        
    }
}