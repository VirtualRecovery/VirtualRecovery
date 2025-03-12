// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    internal class HandGrabController : MonoBehaviour {
        
        
        
        private GameObject m_thumb = null;
        private Collider m_thumbCollider = null;
        private GameObject m_touchedObject = null;
        private Collider m_touchedObjectCollider = null;
        private int m_fingerCount = 0;
        private FixedJoint m_fixedJoint = null;
        
        

        public void Awake() {
        }

        public void Update() {
            if (IsObjectInHand() && !m_fixedJoint) {
                CreateJoint();
            }

            if (!IsObjectInHand() && m_touchedObject&& m_fixedJoint) {
                RemoveJoint();
                m_fingerCount = 0;
            }
        }

        private void CreateJoint() {
            Vector3 thumbPos = m_thumb.transform.position;
            Vector3 objPos = m_touchedObject.transform.position;
            Vector3 objTouchPoint = m_touchedObjectCollider.ClosestPointOnBounds(thumbPos);
            Vector3 thumbTouchPoint = m_thumbCollider.ClosestPointOnBounds(objPos);
            m_fixedJoint = m_touchedObject.AddComponent<FixedJoint>();
            m_fixedJoint.connectedBody = m_thumb.GetComponent<Rigidbody>();
            m_fixedJoint.anchor = objTouchPoint;
            m_fixedJoint.connectedAnchor = thumbTouchPoint;
            m_fixedJoint.enableCollision = true;
            m_touchedObject.GetComponent<Rigidbody>().useGravity = false;
            //m_thumbTouchedObject.GetComponent<Rigidbody>().isKinematic = true;
            //m_thumbTouchedObject.layer = m_thumb.layer;
            Debug.Log(objTouchPoint+" " + thumbTouchPoint +" Attached");
        }

        private void RemoveJoint() {
            Destroy(m_fixedJoint);
            m_touchedObject.GetComponent<Rigidbody>().useGravity = true;
            //m_thumbTouchedObject.layer = 0;
            //m_thumbTouchedObject.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Released");
        }

        public void HandleThumbTouch(GameObject thumb, GameObject obj) {
            m_touchedObject = obj;
            m_touchedObjectCollider = obj.GetComponent<Collider>();
            m_thumb = thumb;
            m_thumbCollider = m_thumb.GetComponent<Collider>();
        }

        public void HandleThumbRelease(GameObject thumb, GameObject obj) {
            m_touchedObject = null;
            m_touchedObjectCollider = null;
            m_thumb = null;
            m_thumbCollider = null;
        }

        public void HandleFingerTouch(GameObject obj) {
            if (m_touchedObject && m_touchedObject == obj) {
                m_fingerCount++;
            }
        }

        public void HandleFingerRelease(GameObject obj) {
            if (m_touchedObject && m_touchedObject == obj) {
                m_fingerCount--;
            }
        }

        private bool IsObjectInHand() {
            return m_thumb && m_touchedObject && m_fingerCount > 0;
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