// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VirtualRecovery.Core.Managers {
    internal class HandGrabController : MonoBehaviour {
        
        
        
        private GameObject m_thumb = null;
        private Collider m_thumbCollider = null;
        private GameObject m_thumbTouchedObject = null;
        private Collider m_touchedCollider = null;
        private int m_fingerCount = 0;
        private FixedJoint m_fixedJoint = null;
        
        

        public void Awake() {
        }

        public void Update() {
            if (IsObjectInHand() && !m_fixedJoint) {
                CreateJoint();
            }

            if (!IsObjectInHand() && m_thumbTouchedObject&& m_fixedJoint) {
                RemoveJoint();
                m_fingerCount = 0;
            }
        }

        private void CreateJoint() {
            Vector3 thumbPos = m_thumb.transform.position;
            Vector3 objPos = m_thumbTouchedObject.transform.position;
            Vector3 objTouchPoint = m_touchedCollider.ClosestPointOnBounds(thumbPos);
            Vector3 thumbTouchPoint = m_thumbCollider.ClosestPointOnBounds(objPos);
            m_fixedJoint = m_thumbTouchedObject.AddComponent<FixedJoint>();
            m_fixedJoint.connectedBody = m_thumb.GetComponent<Rigidbody>();
            m_fixedJoint.anchor = objTouchPoint;
            m_fixedJoint.connectedAnchor = thumbTouchPoint;
            m_fixedJoint.enableCollision = true;
            m_thumbTouchedObject.GetComponent<Rigidbody>().useGravity = false;
            //m_thumbTouchedObject.GetComponent<Rigidbody>().isKinematic = true;
            //m_thumbTouchedObject.layer = m_thumb.layer;
            Debug.Log(objTouchPoint+" " + thumbTouchPoint +" Attached");
        }

        private void RemoveJoint() {
            Destroy(m_fixedJoint);
            m_thumbTouchedObject.GetComponent<Rigidbody>().useGravity = true;
            //m_thumbTouchedObject.layer = 0;
            //m_thumbTouchedObject.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Released");
        }

        public void HandleThumbTouch(GameObject thumb, GameObject obj) {
            m_thumbTouchedObject = obj;
            m_touchedCollider = obj.GetComponent<Collider>();
            m_thumb = thumb;
            m_thumbCollider = m_thumb.GetComponent<Collider>();
        }

        public void HandleThumbRelease(GameObject thumb, GameObject obj) {
            m_thumbTouchedObject = null;
            m_touchedCollider = null;
            m_thumb = null;
            m_thumbCollider = null;
        }

        public void HandleFingerTouch(GameObject obj) {
            if (m_thumbTouchedObject && m_thumbTouchedObject == obj) {
                m_fingerCount++;
            }
        }

        public void HandleFingerRelease(GameObject obj) {
            if (m_thumbTouchedObject && m_thumbTouchedObject == obj) {
                m_fingerCount--;
            }
        }

        private bool IsObjectInHand() {
            return m_thumb && m_thumbTouchedObject && m_fingerCount > 0;
        }

        // public void HandleFingerTouch(Collider collider) {
        //     if (collider.gameObject.CompareTag(m_tagFingerTip)) {
        //         m_fingerCount++;
        //     }
        //     if (collider.gameObject.CompareTag(m_tagThumbTip)) {
        //         m_thumbTouched = true;
        //         m_thumbTransform = collider.transform;
        //     }
        // }
        //
        // public void HandleFingerRelease(Collider collider) {
        //     if (collider.CompareTag(m_tagFingerTip)) {
        //         m_fingerCount--;
        //     }
        //     if (collider.CompareTag(m_tagThumbTip)) {
        //         m_thumbTouched = false;
        //         m_thumbTransform = null;
        //     }
        //     
        // }

        // public bool IsObjectGrabbed() {
        //     return m_thumbTouched && m_fingerCount > 0;
        // }
    }
}