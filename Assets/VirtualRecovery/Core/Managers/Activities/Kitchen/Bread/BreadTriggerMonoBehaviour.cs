// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 12/08/2025
//  */

using System;
using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Bread {
    internal class BreadTriggerMonoBehaviour : MonoBehaviour {
        private BreadActivity m_breadActivity;
        
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Knife")) {
                m_breadActivity.SliceCut();
                var slice = transform.parent;
                if (slice != null) {
                    Destroy(slice.gameObject);
                }
                GetComponent<Collider>().enabled = false;
            }
        }
        
        public void SetBreadActivity(BreadActivity breadActivity) {
            m_breadActivity = breadActivity;
        }
    }
}