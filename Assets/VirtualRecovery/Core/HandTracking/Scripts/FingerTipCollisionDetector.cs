// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 28/02/2025
//  */

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class FingerTipCollisionDetector : MonoBehaviour {
        [SerializeField] private HandGrabController mHandGrabController;
        [SerializeField] private HandGrabController.Fingers mFinger;
        private String m_grabbableTag = "Grabbable";
        private void OnCollisionEnter(Collision collision) {
//            Debug.Log("Finger collision: " + collision.gameObject.name + " tip: " + this.gameObject.name +
//                      "-" + mFinger.ToString() + " current joint rotation: " );

            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                Debug.Log(collision.gameObject.name);
                mHandGrabController.FingerTipCollisionEnter(this.gameObject, mFinger, collision.gameObject);
            }
        }

        private void OnCollisionExit(Collision collision) {
//            Debug.Log("finger collision exit: " + collision.gameObject.name +
//                      "-" + mFinger.ToString());
            if (collision.gameObject.CompareTag(m_grabbableTag)) {
                mHandGrabController.FingerTipCollisionExit(this.gameObject, mFinger);
            }
        }
        
    }
}