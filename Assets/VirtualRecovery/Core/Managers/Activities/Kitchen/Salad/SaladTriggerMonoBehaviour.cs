// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 03/09/2025
//  */

using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Salad {
    internal class SaladTriggerMonoBehaviour : MonoBehaviour {
        private SaladActivity m_saladActivity;
        private bool m_wasTriggered = false;
        public void OnTriggerEnter(Collider other) {
            if (!m_wasTriggered && other.name == "spoon_head") {
                m_wasTriggered = true;
                m_saladActivity.CreateNextTrigger();
                GameObject.Destroy(this.gameObject);
            }
        }
        
        public void SetSaladActivity(SaladActivity saladActivity) {
            m_saladActivity = saladActivity;
        }
    }
}