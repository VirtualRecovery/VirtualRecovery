// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 08/09/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities {
    internal class ReturnToInitialPosition : MonoBehaviour {
        private Vector3 m_initialPosition;
        private Quaternion m_initialRotation;
        
        private void Start() {
            m_initialPosition = transform.position;
            m_initialRotation = transform.rotation;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.name == "Podłoga") {
                transform.position = m_initialPosition;
                transform.rotation = m_initialRotation;
            }
        }
    }
}