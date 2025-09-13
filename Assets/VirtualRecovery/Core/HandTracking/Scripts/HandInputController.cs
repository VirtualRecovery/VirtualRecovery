// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 05/07/2025
//  */

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class HandInputController : MonoBehaviour {
        [SerializeField] HandGrabDetector leftHandGrabDetector;
        [SerializeField] HandGrabDetector rightHandGrabDetector;
        [SerializeField] PatientPinchDetection patientPinchDetection;
        private bool isGrabbedLeft = false;
        private bool isGrabbedRight = false;
        private bool isPinching = false;
        public void Update() {
            if (!isPinching) {
                isGrabbedLeft = leftHandGrabDetector.CheckHandGrab();   
                isGrabbedRight = rightHandGrabDetector.CheckHandGrab();   
            }

            if (patientPinchDetection && !isGrabbedLeft && !isGrabbedRight) {
                isPinching = patientPinchDetection.CheckPinch();   
            }
        }
    }
}