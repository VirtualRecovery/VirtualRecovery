// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 12/08/2025
//  */

using System;
using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf {
    public class ShelfTriggerMonoBehaviour : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.name == "plate") {
                GameManager.Instance.EndSession();
            }
        }
    }
}