// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities {
    internal abstract class BaseActivity {
        private static GameObject player => GameObject.Find("PlayerDoubleHandControlMechnic")
                                     ?? throw new InvalidOperationException("No player found.");
        private static GameObject VROrig => GameObject.Find("XR Origin (XR Rig)")
                                           ?? throw new InvalidOperationException("No VR rig found.");
        
        protected string m_sessionEndingObjectName;
        protected Type m_sessionEndingObjectType;
        protected Vector3 m_playerStartPosition;
        protected Quaternion m_playerStartRotation;

        protected BaseActivity(
            string sessionEndingObjectName,
            Type sessionEndingObjectType,
            Vector3 playerStartPosition,
            Quaternion playerStartRotation)
        {
            m_sessionEndingObjectName = sessionEndingObjectName;
            m_sessionEndingObjectType = sessionEndingObjectType;
            m_playerStartPosition     = playerStartPosition;
            m_playerStartRotation     = playerStartRotation;
        }
        
        public void Load(DifficultyLevel difficultyLevel, BodySide bodySide) {
            var actions = new Dictionary<DifficultyLevel, Action> {
                { DifficultyLevel.Łatwy, () => LoadEasy() },
                { DifficultyLevel.Średni, () => LoadMedium() },
                { DifficultyLevel.Trudny, () => LoadHard() }
            };

            if (actions.TryGetValue(difficultyLevel, out var action)) {
                action();
            }
            else {
                throw new ArgumentException($"Invalid difficulty level: {difficultyLevel}");
            }

            SetupBodySide(bodySide);
            SetupCameraPosition();
            AttachTrigger();
        }
        
        protected abstract void LoadEasy();
        protected abstract void LoadMedium();
        protected abstract void LoadHard();
        
        protected abstract void SetupBodySide(BodySide bodySide);
        
        private void SetupCameraPosition() {
            player.transform.position = m_playerStartPosition;
            VROrig.transform.rotation = m_playerStartRotation;
        }

        private void AttachTrigger() {
            var gameObject = GameObject.Find(m_sessionEndingObjectName);
            if (gameObject == null) {
                Debug.LogError($"No {m_sessionEndingObjectName} GameObject found.");
                return;
            }
            gameObject.AddComponent(m_sessionEndingObjectType);
        }
    }
}