// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 02/09/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Scenes {
    public class FollowPlayerCamera : MonoBehaviour {
        [SerializeField] private Camera playerCamera;
        void LateUpdate() {
            transform.position = playerCamera.transform.position;
            transform.rotation = playerCamera.transform.rotation;
            GetComponent<Camera>().fieldOfView = playerCamera.fieldOfView;
        }
    }
}