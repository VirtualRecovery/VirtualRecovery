// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 02/07/2025
//  */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class PatientPinchDetection : MonoBehaviour {
        [SerializeField] private float threshold;
        private XRHandSubsystem m_handSubsystem;
        private Canvas m_pauseCanvas;
        private bool m_isPinchingLeftHand = false;
        private bool m_isPinchingRightHand = false;
        
        private void Start() {
            m_pauseCanvas =  GameObject.FindWithTag("PlayerPauseCanvas").GetComponent<Canvas>();
            if (m_handSubsystem == null)
            {
                var subsystems = new List<XRHandSubsystem>();
                SubsystemManager.GetSubsystems(subsystems);
                if (subsystems.Count > 0)
                {
                    m_handSubsystem = subsystems[0];
                }
            }
        }

        public bool CheckPinch() {
            if (m_handSubsystem == null) {
                return false;
            }
            
            XRHand leftHand = m_handSubsystem.leftHand;
            XRHand rightHand = m_handSubsystem.rightHand;

            m_isPinchingLeftHand = CheckHand(leftHand, m_isPinchingLeftHand);
            m_isPinchingRightHand = CheckHand(rightHand, m_isPinchingRightHand);
            if (m_isPinchingLeftHand || m_isPinchingRightHand) {
                return true;
            }
            return false;
        }

        private bool CheckHand(XRHand hand, bool isPinching) {
            if (hand.isTracked) {
                XRHandJoint thumbTip = hand.GetJoint(XRHandJointID.ThumbTip);
                XRHandJoint indexTip = hand.GetJoint(XRHandJointID.IndexTip);
                if (
                    thumbTip.TryGetPose(out Pose thumbPose) &&
                    indexTip.TryGetPose(out Pose indexPose)
                ) {
                    float distance = Vector3.Distance(thumbPose.position, indexPose.position);
                    if (distance < threshold && isPinching) {
                        Debug.Log("Pinch detected!");
                        OnPinchDetection();
                        return true;
                    }
                    Debug.Log("Pinch ended!");
                    return false;
                }
            }
            return false;
        }
        
        private void OnPinchDetection() {
            if (!GameManager.Instance.IsGamePaused()) {
                Debug.Log("Game is paused");
                GameManager.Instance.PauseGame();
                m_pauseCanvas.enabled = true;
            }
            else {
                Debug.Log("Game is resumed");
                GameManager.Instance.ResumeGame();
                m_pauseCanvas.enabled = false;
            }
        }
    }
}