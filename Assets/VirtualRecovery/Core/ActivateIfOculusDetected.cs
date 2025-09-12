// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 12/09/2025
//  */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace VirtualRecovery.Core {
    internal class ActivateIfOculusDetected : MonoBehaviour {
        [SerializeField] private GameObject target;

        private void Awake() {
            if (target == null) target = gameObject;

            bool isOculus = IsOculusHeadsetConnected();

            target.SetActive(isOculus);
            Debug.Log($"[ActivateIfOculus] {target.name} active={isOculus}");
        }

        private bool IsOculusHeadsetConnected() {
            var displays = new List<XRDisplaySubsystem>();
            SubsystemManager.GetSubsystems(displays);

            foreach (var d in displays) {
                if (d.running) {
                    if (d.SubsystemDescriptor.id.ToLower().Contains("oculus"))
                        return true;
                }
            }

            if (XRSettings.isDeviceActive &&
                XRSettings.loadedDeviceName.ToLower().Contains("oculus")) {
                return true;
            }
            return false;
        }
    }
}