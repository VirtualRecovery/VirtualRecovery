// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 02/07/2025
//  */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using VirtualRecovery.Core.Scenes.AbstractActivity;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class PatientPinchDetection : MonoBehaviour {
        [Header("Pinch Settings")]
        [SerializeField, Min(0f)] private float pinchThreshold = 0.025f;
        [SerializeField, Min(0f)] private float releaseThreshold = 0.035f;
        [SerializeField, Min(0f)] private float toggleCooldownSeconds = 0.35f;
        
        [SerializeField, Min(0f)] private float nonPinchSeparationStart = 0.045f;
        [SerializeField, Min(0f)] private float nonPinchSeparationRelease = 0.035f;

        [SerializeField] private ActivityCanvasChanger activityCanvasChanger;

        private XRHandSubsystem m_handSubsystem;

        private bool m_leftPinching;
        private bool m_rightPinching;

        private bool m_leftWasPinching;
        private bool m_rightWasPinching;

        private float m_lastToggleTime = -10f;

        private void Awake() {
            if (releaseThreshold <= pinchThreshold)
                releaseThreshold = pinchThreshold + 0.01f;

            if (nonPinchSeparationRelease > nonPinchSeparationStart)
                nonPinchSeparationRelease = Mathf.Max(0f, nonPinchSeparationStart - 0.01f);

            TryGetHandSubsystem();
        }

        public bool CheckPinch() {
            if (m_handSubsystem == null || (!m_handSubsystem.leftHand.isTracked && !m_handSubsystem.rightHand.isTracked))
                return false;

            m_leftWasPinching = m_leftPinching;
            m_rightWasPinching = m_rightPinching;

            m_leftPinching  = GetPinchState(m_handSubsystem.leftHand,  m_leftWasPinching);
            m_rightPinching = GetPinchState(m_handSubsystem.rightHand, m_rightWasPinching);

            bool leftRising  = m_leftPinching  && !m_leftWasPinching;
            bool rightRising = m_rightPinching && !m_rightWasPinching;

            if ((leftRising || rightRising) && (Time.unscaledTime - m_lastToggleTime) >= toggleCooldownSeconds) {
                TogglePause();
                m_lastToggleTime = Time.unscaledTime;
                return true;
            }

            return false;
        }

        private bool GetPinchState(XRHand hand, bool wasPinching) {
            if (!hand.isTracked)
                return false;

            if (!TryGetPose(hand, XRHandJointID.ThumbTip, out var thumbPose) ||
                !TryGetPose(hand, XRHandJointID.IndexTip, out var indexPose))
                return false;

            float indexThumb = Vector3.Distance(thumbPose.position, indexPose.position);

            bool basePinch = wasPinching
                ? indexThumb <= releaseThreshold
                : indexThumb <  pinchThreshold;

            if (!basePinch)
                return false;

            float sepStart   = nonPinchSeparationStart;
            float sepRelease = nonPinchSeparationRelease;

            float requiredSeparation = wasPinching ? sepRelease : sepStart;

            bool othersApart = OtherFingersClearlyApart(hand, thumbPose.position, requiredSeparation);

            return othersApart;
        }

        private static bool OtherFingersClearlyApart(XRHand hand, Vector3 thumbPos, float minSeparation) {
            bool apartMiddle = TryGetDistance(hand, XRHandJointID.MiddleTip, thumbPos, out float dM) && dM >= minSeparation;
            bool apartRing   = TryGetDistance(hand, XRHandJointID.RingTip,   thumbPos, out float dR) && dR >= minSeparation;
            bool apartLittle = TryGetDistance(hand, XRHandJointID.LittleTip, thumbPos, out float dL) && dL >= minSeparation;

            return apartMiddle && apartRing && apartLittle;
        }

        private static bool TryGetDistance(XRHand hand, XRHandJointID id, Vector3 refPos, out float distance) {
            distance = 0f;
            if (!TryGetPose(hand, id, out var pose)) return false;
            distance = Vector3.Distance(refPos, pose.position);
            return true;
        }

        private static bool TryGetPose(XRHand hand, XRHandJointID id, out Pose pose) {
            var joint = hand.GetJoint(id);
            if (joint.TryGetPose(out pose)) return true;
            pose = default;
            return false;
        }

        private void TogglePause() {
            activityCanvasChanger.ChangeCanvas(new ActivityEventTypeWrapper(ActivityEventType.PauseTriggered));
        }

        private void TryGetHandSubsystem() {
            if (m_handSubsystem != null) return;

            var subsystems = new List<XRHandSubsystem>();
            SubsystemManager.GetSubsystems(subsystems);
            if (subsystems.Count > 0) {
                m_handSubsystem = subsystems[0];
                if (!m_handSubsystem.running)
                    m_handSubsystem.Start();
            } else {
                Debug.LogWarning("XRHandSubsystem not found. Enable XR Hands (OpenXR) in project settings.");
            }
        }
    }
}