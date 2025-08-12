// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf {
    internal class ShelfActivity : AbstractActivity {
        
        private float m_easyLevelHeight = -0.05f;
        private float m_mediumLevelHeight = 0.05f;
        private float m_hardLevelHeight = 0.2f;
        
        private Vector3 m_startPosition = new Vector3(-1.96000004f,-1.64999998f,1.78999996f);
        private Quaternion m_startRotation = new Quaternion(0,0,0,1);
        
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

        private void ChangeShelfHeight(float height) {
            var shelf = GameObject.Find("shelf");
            if (shelf == null) {
                Debug.LogError($"No shelf found");
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

            
            shelf.transform.position = new Vector3(shelf.transform.position.x, height, shelf.transform.position.z);
            shelf.AddComponent<ShelfTriggerMonoBehaviour>();
        }

        protected override void LoadEasy(BodySide bodySide) {
            ChangeShelfHeight(m_easyLevelHeight);
        }

        protected override void LoadMedium(BodySide bodySide) {
            ChangeShelfHeight(m_mediumLevelHeight);
        }

        protected override void LoadHard(BodySide bodySide) {
            ChangeShelfHeight(m_hardLevelHeight);
        }
    }
}