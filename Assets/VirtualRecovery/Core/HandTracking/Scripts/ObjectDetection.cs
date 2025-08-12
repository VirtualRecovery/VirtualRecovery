using System;
using UnityEngine;


namespace VirtualRecovery.Core.HandTracking.Scripts {
    public class ObjectDetection : MonoBehaviour
    {
        [SerializeField] String objectName;

        private void OnTriggerEnter(Collider other) {
            //Debug.Log("PRZEDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD " + other.gameObject.name);
            if (other.gameObject.name == objectName) {
                //Debug.Log("WYKRYTOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO " + other.gameObject.name);
            }
        }

        private void OnTriggerExit(Collider other) {
            //Debug.Log("OPUSZCZONOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO: " + other.gameObject.name);
        }
    }
}

