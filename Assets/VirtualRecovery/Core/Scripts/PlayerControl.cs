using System;
using Unity.Mathematics;
using UnityEngine;

namespace VirtualRecovery.Core.Scripts {
    internal class PlayerControl : MonoBehaviour {

        public GameObject target;
        public float moveSpeed;
        public float rotationSpeed;
        private Rigidbody m_rb;

        public void Start() {
            m_rb = GetComponent<Rigidbody>();
            //m_rb.position = target.transform.position;
        }
        public void FixedUpdate() {
            //transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
            m_rb.linearVelocity = (target.transform.position - m_rb.position) / Time.fixedDeltaTime;
            Quaternion rotationDifference = target.transform.rotation * Quaternion.Inverse(m_rb.rotation);
            rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);
            Vector3 rotationDifferenceInDegrees = angle * axis;
            m_rb.angularVelocity = rotationDifferenceInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;
            //m_rb.ResetInertiaTensor();
        }
            
        
    }

}
