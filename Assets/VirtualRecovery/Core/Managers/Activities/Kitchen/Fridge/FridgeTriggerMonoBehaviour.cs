// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 12/08/2025
//  */

using System;
using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Fridge {
    public class FridgeTriggerMonoBehaviour : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {

            if (other.name == "fridgeDoor") {
                Debug.Log("Fridge Door");
                GameManager.Instance.EndSession();
            }
        }
    }
}