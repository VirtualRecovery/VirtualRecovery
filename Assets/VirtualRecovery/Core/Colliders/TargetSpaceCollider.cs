// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 13/03/2025
//  */

using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Colliders {
    internal class TargetSpaceCollider : MonoBehaviour {
        [SerializeField] public string targetSpaceId;
        [SerializeField] public float touchDuration = 2f;
        private bool m_isBeingTouched = false;
        private float m_touchTimer = 0f;
        private string m_touchedItemId;
        
        private void OnTriggerEnter(Collider other)
        {
            // TODO: Check tag
            if (other.CompareTag(""))
            {
                m_isBeingTouched = true;
                
                ItemCollider targetSpaceCollider = other.GetComponent<ItemCollider>();
                if (targetSpaceCollider != null)
                {
                    m_touchedItemId = targetSpaceCollider.itemId;
                    Debug.Log($"Dotknięto obiekt z ID: {m_touchedItemId}");
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            // TODO: Check tag 
            if (other.CompareTag(""))
            {
                m_isBeingTouched = false;
                m_touchTimer = 0f;
                
                ItemCollider targetSpaceCollider = other.GetComponent<ItemCollider>();
                if (targetSpaceCollider != null)
                {
                    string otherObjectId = targetSpaceCollider.itemId;
                    Debug.Log($"Obiekt o ID {otherObjectId} opuścił collider");
                }
            }
        }
        
        private void Update()
        {
            if (m_isBeingTouched)
            {
                m_touchTimer += Time.deltaTime;
                if (m_touchTimer >= touchDuration)
                {
                    GameManager.Instance.CompleteStage(targetSpaceId, m_touchedItemId);
                    m_isBeingTouched = false;
                }
            }
        }
    }
}