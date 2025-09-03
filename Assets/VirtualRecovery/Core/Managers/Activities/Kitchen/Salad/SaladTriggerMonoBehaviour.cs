// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 03/09/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Salad {
    internal class SaladTriggerMonoBehaviour : MonoBehaviour {
        private SaladActivity m_saladActivity;
        public void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Spoon")) {
                m_saladActivity.CreateNextTrigger();
                GameObject.Destroy(this.gameObject);
            }
        }
        
        public void SetSaladActivity(SaladActivity saladActivity) {
            m_saladActivity = saladActivity;
        }
    }
}