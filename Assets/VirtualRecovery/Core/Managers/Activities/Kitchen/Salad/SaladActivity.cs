// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 03/09/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Salad {
    internal class SaladActivity : BaseActivity {
        
        private Dictionary<DifficultyLevel, float> m_triggerCreationRadiusByDifficultyLevel = new() {
            { DifficultyLevel.Łatwy, 0.053f },
            { DifficultyLevel.Średni, 0.095f },
            { DifficultyLevel.Trudny, 0.142f }
        };
        
        private float m_triggerCreationRadius;
        
        private int m_fullCircles = 3;
        private int m_triggersInCircle = 12;
        private int m_triggersCreated = 0;
        
        private GameObject spoon => GameObject.Find("spoon")
                                      ?? throw new InvalidOperationException("No Spoon found.");
        
        private GameObject salad => GameObject.Find("salad")
                                      ?? throw new InvalidOperationException("No Salad Bowl found.");

        private GameObject saladTrigerPrefab => GameObject.Find("SaladTrigger")
                                      ?? throw new InvalidOperationException("No Salad Trigger placeholder found.");
        
        public SaladActivity()
            : base(
                "Tylna ściana",
                typeof(SaladTriggerMonoBehaviour),
                new Vector3(-1.41400003f,-1.64999998f,0.753000021f),
                new Quaternion(0, -1, 0, 0)) {
        }
        
        public void CreateNextTrigger() {
            if (m_triggersCreated >= m_triggersInCircle * m_fullCircles) {
                GameManager.Instance.EndSession();
            }
            
            var angle = 2 * Mathf.PI * (m_triggersCreated % m_triggersInCircle) / m_triggersInCircle;
            var x = m_triggerCreationRadius * Mathf.Cos(angle);
            var z = m_triggerCreationRadius * Mathf.Sin(angle);
            var position = new Vector3(salad.transform.position.x + x, -0.234f, salad.transform.position.z + z);

            var trigger = GameObject.Instantiate(saladTrigerPrefab, position, new Quaternion(0,0,0,1));
            trigger.name = m_triggersCreated + 1 == m_triggersInCircle ? "FinalSaladTrigger" : $"SaladTrigger_{m_triggersCreated + 1}";
            trigger.GetComponent<SaladTriggerMonoBehaviour>().SetSaladActivity(this);
            
            m_triggersCreated++;
        }
        
        protected override void LoadEasy() {
            m_triggerCreationRadius = m_triggerCreationRadiusByDifficultyLevel[DifficultyLevel.Łatwy];
            CreateNextTrigger();
        }

        protected override void LoadMedium() {
            m_triggerCreationRadius = m_triggerCreationRadiusByDifficultyLevel[DifficultyLevel.Średni];
            CreateNextTrigger();
        }

        protected override void LoadHard() {
            m_triggerCreationRadius = m_triggerCreationRadiusByDifficultyLevel[DifficultyLevel.Trudny];
            CreateNextTrigger();
        }
        
        protected override void SetupBodySide(BodySide bodySide) {
            switch (bodySide) {
                case BodySide.Lewa:
                    spoon.transform.localPosition = new Vector3(-0.00899999961f,-0.319999993f,-1.21099997f);
                    break;
                case BodySide.Prawa:
                    spoon.transform.localPosition = new Vector3(-0.683000028f,-0.319999993f,-1.21099997f);
                    break;
                default:
                    throw new InvalidOperationException("Invalid body side.");
            }
        }
    }
}