// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 12/08/2025
//  */

using System;
using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Stove {
    public class StoveTriggerMonoBehaviour : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Pot")) {
                GameManager.Instance.EndSession();
            }
        }
    }
}