// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 08/09/2025
//  */

using System;
using UnityEngine;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Soup {
    internal class SoupTriggerMonoBehaviour :MonoBehaviour {
        private SoupActivity m_soupActivity;
        
        private bool m_hasSoup = false;

        [SerializeField] public GameObject soup;
        private GameObject soupPlate => GameObject.Find("soupPlate")
                                        ?? throw new InvalidOperationException("No SoupPlate found.");
        
        private void Update() {
            if (m_hasSoup && IsLadleTilted() && IsLadleAbovePlate()) {
                m_hasSoup = false;
                soup.SetActive(false);
                m_soupActivity.SoupPoured();
            }
        }
        
        private bool IsLadleTilted() {
            return transform.localRotation.eulerAngles.x < 230;
        }
        
        private bool IsLadleAbovePlate() {
            return Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(soupPlate.transform.position.x, soupPlate.transform.position.z)
            ) < 0.1f;
        }
        
        public void SetSoupActivity(SoupActivity soupActivity) {
            m_soupActivity = soupActivity;
        }
        
        public void OnTriggerEnter(Collider other) {
            if (other.gameObject.name.StartsWith("SoupTrigger") && !m_hasSoup) {
                m_hasSoup = true;
                soup.SetActive(true);
                GameObject.Destroy(other.gameObject);
            }
        }
    }
}