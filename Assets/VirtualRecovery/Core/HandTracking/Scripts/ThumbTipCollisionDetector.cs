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
        
        [FormerlySerializedAs("mHandGrabController")] [SerializeField] private HandGrabDetector mHandGrabDetector;
        private String m_grabbableTag = "Grabbable";
        
        private void OnCollisionEnter(Collision collision) {
//            Debug.Log("Thumb collision enter:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                mHandGrabDetector.ThumbTipCollisionEnter(this.gameObject, collision.gameObject, collision);    
            }
        }

        private void OnCollisionExit(Collision collision) { 
            // Debug.Log("Thumb collision exit:" + collision.gameObject.name);
            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                mHandGrabDetector.ThumbTipCollisionExit(this.gameObject);
            }
        }
        
    }
}