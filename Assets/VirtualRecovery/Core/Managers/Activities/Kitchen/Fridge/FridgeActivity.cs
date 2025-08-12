// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Fridge {
    internal class FridgeActivity : AbstractActivity {
        
        private float m_easyLevelHeight = -0.00405f;
        private float m_mediumLevelHeight = 0.00059f;
        private float m_hardLevelHeight = 0.00417f;
        private Vector3 m_startPosition = new Vector3(-2.26399994f,-1.64999998f,0.996999979f);
        private Quaternion m_startRotation = new Quaternion(0,-1,0,0);
        
        public override void Load(DifficultyLevel difficultyLevel, BodySide bodySide) {
            var actions = new Dictionary<DifficultyLevel, Action> {
                { DifficultyLevel.Łatwy, () => LoadEasy(bodySide) },
                { DifficultyLevel.Średni, () => LoadMedium(bodySide) },
                { DifficultyLevel.Trudny, () => LoadHard(bodySide) }
            };

            if (actions.TryGetValue(difficultyLevel, out var action)) {
                action();
            }
            else {
                throw new ArgumentException($"Invalid difficulty level: {difficultyLevel}");
            }
        }
        
        private void ChangeHandleHeight(float height) {
            var fridgeHandle = GameObject.Find("Uchwyt góra");
            if (fridgeHandle == null) {
                Debug.LogError($"No fridge handle found");
                return;
            }
            var fridgDoorCollider = GameObject.Find("FridgeOpenTriggerCollider");
            if (fridgDoorCollider == null) {
                Debug.LogError($"No fridge collider found");
                return;
            }
            
            var player = GameObject.Find("PlayerDoubleHandControlMechnic");
            if (player == null) {
                Debug.LogError($"No player collider found");
                return;
            }
            player.transform.position = m_startPosition;
            
            var vr = GameObject.Find("XR Origin (XR Rig)");
            if (vr == null) {
                Debug.LogError($"No player vr found");
                return;
            }
            vr.transform.rotation = m_startRotation;
            
            
            var connectedBody = fridgeHandle.GetComponent<FixedJoint>().connectedBody;
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = null;
            fridgeHandle.transform.localPosition  = new Vector3(fridgeHandle.transform.localPosition .x, fridgeHandle.transform.localPosition .y, height);
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = connectedBody;
            fridgDoorCollider.AddComponent<FridgeTriggerMonoBehaviour>();
        }
        
        protected override void LoadEasy(BodySide bodySide) {
            ChangeHandleHeight(m_easyLevelHeight);
        }

        protected override void LoadMedium(BodySide bodySide) {
            ChangeHandleHeight(m_mediumLevelHeight);
        }

        protected override void LoadHard(BodySide bodySide) {
            ChangeHandleHeight(m_hardLevelHeight);
        }
    }
}