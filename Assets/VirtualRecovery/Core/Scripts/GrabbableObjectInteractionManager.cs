// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using System;
using UnityEngine;
using UnityEngine.XR;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scripts {
    internal class GrabbableObjectInteractionManager : MonoBehaviour {
        
        private HandGrabController m_handGrabController;

        private Rigidbody m_rigidBody;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other) {
            //m_handGrabController.HandleFingerTouch(other);
        }

        private void OnTriggerExit(Collider other) {
            //m_handGrabController.HandleFingerRelease(other);
        }

        private void OnCollisionEnter(Collision collision) {
            // if (collision.collider.gameObject.CompareTag(m_tagFingerTip)) {
            //     m_fingerCount++;
            // }
            // //Debug.Log(m_fingerCount);
            // if (m_fingerCount >= 2) {
            //     EnableRagdoll(collision.collider.gameObject);
            // }
        }

        private void OnCollisionStay(Collision other) {
            // if (m_fingerCount >= 2) {
            //     Debug.Log("grabbed");
            // }
        }

        private void OnCollisionExit(Collision collision) {
            // if (collision.collider.gameObject.CompareTag(m_tagFingerTip)) {
            //     m_fingerCount--;
            // }
            // //Debug.Log(m_fingerCount);
            // if (m_fingerCount >= 2) {
            //     DisableRagdoll();
            // }
        }
    }
}
