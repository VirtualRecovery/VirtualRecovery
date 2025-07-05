// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Ezra Krępa
//  * Created on: 27/01/2025
//  */

using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    internal class HandGrabDetector : MonoBehaviour {
        public enum Fingers {
            IndexFinger,
            MiddleFinger,
            RingFinger,
            PinkyFinger,
            None
        }

        public enum State {
            HandEmpty,
            ObjectGrabbed,
            ObjectReleased // temporary state to allow the object to fall
        }
        
        private State m_state = State.HandEmpty;
        private GameObject m_handHeldObject;
        private FixedJoint m_objectFixedJoint;
        private int m_oldHandHeldObjectLayer;
        private string m_handHeldObjectsLayerName = "HandHeldObjects";
        private bool[] m_fingersTouchingHandHeldObject = { false, false, false, false };
        
        private static int s_mfingerCount = 4; 
        [SerializeField] private float mGrabSensitivity = 0.1f;
        [SerializeField] private Rigidbody m_handPalmRigidBody;
        [SerializeField] private Transform m_handWristTransform;
        private Vector3 m_handOldPosition;
        private Vector3 m_handPosition;
        private GameObject mThumbTip;
        private Collision m_thumbTipCollision = null;
        private Collider m_thumbTipCollider = null;
        private GameObject m_thumbTipTouchedObject = null;
        private Collider m_thumbTipTouchedObjectCollider = null;
        private FixedJoint m_fixedJoint = null;
        private GameObject[] m_fingerTouchedObjects = { null, null, null, null };
        private Transform[] m_fingersTransform = {null, null, null, null};
        private Quaternion[] m_fingersRotationOnTouch = new Quaternion[s_mfingerCount];
        private bool m_objectInHand = false;
        private Fingers m_fingerHoldingObject = Fingers.None;
        private bool m_thumbTipHoldingObject = false;
       
        
        public void Awake() {
        }

        public bool CheckHandGrab() {
            
            // tracking velocity for throwing
            m_handOldPosition = m_handPosition;
            m_handPosition = m_handWristTransform.position;
            
            // if the hand is empty, check whether it could grab an object
            if (m_state == State.HandEmpty) {
                GrabObject();
            }
            // if an object is grabbed, check if it could be released
            else if (m_state == State.ObjectGrabbed) {
                ReleaseObject();
            }

            if (m_state == State.ObjectGrabbed) {
                return true;
            }
            return false;
        }

        private void GrabObject() {
            bool objectGrabbed = false;
            // if thumb has touched an object
            // check if any of the fingers has done the same
            if (m_thumbTipTouchedObject && mThumbTip) {
                for (int i = 0; i < s_mfingerCount; i++) {
                    // save the proximal joint rotations of fingers touching the same object 
                    /*Debug.Log("Change hand grab for finger: " + i);*/
                    if (m_fingerTouchedObjects[i] && m_fingerTouchedObjects[i] == m_thumbTipTouchedObject) {
                        objectGrabbed = true;
                        m_fingersRotationOnTouch[i] = m_fingersTransform[i].localRotation;
                        m_fingersTouchingHandHeldObject[i] = true;
                        m_handHeldObject = m_thumbTipTouchedObject;
                    }
                }
            }
            // if an object has been touched by both thumb and a finger
            // and attach a fixed joint to the object
            // and change the object layer to HandHeldObjects
            
            
            if (objectGrabbed) {
                // attaching the joint
                m_objectFixedJoint = m_handHeldObject.AddComponent<FixedJoint>();
                //m_objectFixedJoint.connectedBody = mThumbTip.GetComponent<Rigidbody>();
                m_objectFixedJoint.connectedBody = m_handPalmRigidBody;
                // disabling gravity in the hand held object
                m_handHeldObject.GetComponent<Rigidbody>().useGravity = false;
                // changing the object layer and saving the previous one
                m_oldHandHeldObjectLayer = m_handHeldObject.layer;
                //m_handHeldObject.layer = LayerMask.NameToLayer(m_handHeldObjectsLayerName);
                SetLayerRecursively(m_handHeldObject, LayerMask.NameToLayer(m_handHeldObjectsLayerName));
                
                // remember to change the state
                m_state = State.ObjectGrabbed;
                Debug.Log("Hand Grab Controller: Object Grabbed \nFingers grabbing object: " +
                          string.Join(",", m_fingersTouchingHandHeldObject));
                
            }
        }

        void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (obj == null) return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }


        private void ReleaseObject() {
            
            bool objectReleased = false;
            // check if fingers have rotated enough to release the object
            for (int i = 0; i < s_mfingerCount; i++) {
                if (m_fingersTouchingHandHeldObject[i]) {
                    
                    // the bigger the local rotation the further back the finger is rotated
                    // Debug.Log(m_fingersRotationOnTouch[i].x - m_fingersTransform[i].localRotation.x, this);
                    if (m_fingersRotationOnTouch[i].x - m_fingersTransform[i].localRotation.x > mGrabSensitivity) {
                        objectReleased = true;
                    }
                    // if at least one finger is still close enough
                    // do not release
                    // else {
                    //     objectReleased = false;
                    // }
                }
            }
            
            // change the layer back
            // turn gravity back on
            // and remove the joint
            if (objectReleased) {
                
                
                m_handHeldObject.GetComponent<Rigidbody>().useGravity = true;
                m_handHeldObject.GetComponent<Rigidbody>().linearVelocity =
                    (m_handPosition - m_handOldPosition)/0.02f;
                
                Destroy(m_objectFixedJoint);
                m_state = State.ObjectReleased;
                Debug.Log("Hand Grab Controller: Object released" + (m_handOldPosition - m_handPosition)/0.02f);
                StartCoroutine(ChangeStateAfterDelay());
            }
            
        }
        
        private IEnumerator ChangeStateAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            m_state = State.HandEmpty;
            //m_handHeldObject.layer = m_oldHandHeldObjectLayer;
            SetLayerRecursively(m_handHeldObject, m_oldHandHeldObjectLayer);
            Debug.Log("Hand Grab Controller: Hand Empty");
        }

        public void ThumbTipCollisionEnter(GameObject thumb, GameObject touchedObject, Collision collision) {
            m_thumbTipCollision = collision;
            m_thumbTipTouchedObject = touchedObject;
            m_thumbTipTouchedObjectCollider = touchedObject.GetComponent<Collider>();
            mThumbTip = thumb;
            m_thumbTipHoldingObject = true;
            m_thumbTipCollider = thumb.GetComponent<Collider>();
            Debug.Log("thumb tip collision enter with" + m_thumbTipCollider.name);
        }

        public void ThumbTipCollisionExit(GameObject thumb) {
            m_thumbTipHoldingObject = false;
            m_thumbTipTouchedObjectCollider = null;
            mThumbTip = null;
            m_thumbTipCollider = null;
            Debug.Log("thumb tip collision exit with");
        }

        public void FingerTipCollisionEnter(GameObject tipGameObject, Fingers fingerNumber, GameObject touchedGameObject) {
            // getting proximal from tip  => tip -> distal -> intermediate -> proximal -> metacarpal 
            GameObject proximalGameObject = tipGameObject.transform.parent.gameObject. // distal
                transform.parent.gameObject. // intermediate
                transform.parent.gameObject;             // proximal
            
            m_fingersTransform[(int)fingerNumber] = proximalGameObject.transform;
            m_fingerTouchedObjects[(int)fingerNumber] = touchedGameObject;
            Debug.Log(proximalGameObject.name + " collision entered with " + touchedGameObject.name);
        }

        public void FingerTipCollisionExit(GameObject tipGameObject, Fingers fingerNumber) {
            // getting proximal from tip  => tip -> distal -> intermediate -> proximal -> metacarpal 
            GameObject proximalGameObject = tipGameObject.transform.parent.gameObject. // distal
                transform.parent.gameObject. // intermediate
                transform.parent.gameObject;             // proximal

            /*m_fingersTransform[(int)fingerNumber] = null;*/
            m_fingerTouchedObjects[(int)fingerNumber] = null;
            /*if (!m_objectInHand) {
                m_fingerTouchedObjects[(int)fingerNumber] = null;
            }*/
            Debug.Log(proximalGameObject.name + " collision exit");
        }
    }
}