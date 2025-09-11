// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 11/09/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Scenes
{
    public class FaceXRCamera : MonoBehaviour
    {
        [SerializeField] public Transform xrCam;

        [Header("Placement (meters)")]
        [SerializeField] float distance = 0.75f;      // 0.6–0.9 m feels good for poke
        [SerializeField] float verticalOffset = -0.05f;

        [Header("Smoothing")]
        [SerializeField] float posLerp = 12f;
        [SerializeField] float rotLerp = 12f;

        [Header("Follow Mode")]
        [SerializeField] bool followContinuously = true;

        bool _placedOnce;

        void LateUpdate()
        {
            if (!xrCam) return;

            var flatForward = Vector3.ProjectOnPlane(xrCam.forward, Vector3.up).normalized;
            if (flatForward.sqrMagnitude < 0.0001f) flatForward = xrCam.transform.forward;

            var targetPos = xrCam.position + flatForward * distance + Vector3.up * verticalOffset;
            var targetRot = Quaternion.LookRotation(flatForward, Vector3.up);

            bool shouldUpdate = followContinuously || !_placedOnce;

            if (shouldUpdate)
            {
                // exponential smoothing
                float p = 1f - Mathf.Exp(-posLerp * Time.deltaTime);
                float r = 1f - Mathf.Exp(-rotLerp * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPos, p);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, r);
            }

            if (!_placedOnce) _placedOnce = true;
        }
        
        public void RepositionNow()
        {
            _placedOnce = false;
        }
    }
}
