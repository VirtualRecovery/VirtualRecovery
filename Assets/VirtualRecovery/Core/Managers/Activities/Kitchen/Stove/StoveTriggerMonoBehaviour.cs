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
            if (other.name =="pot_base") {
                GameManager.Instance.EndSession();
            }
        }
    }
}