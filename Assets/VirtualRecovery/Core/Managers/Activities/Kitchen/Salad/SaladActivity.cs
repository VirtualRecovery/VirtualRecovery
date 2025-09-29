// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 03/09/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;
using Random = UnityEngine.Random;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Salad {
    internal class SaladActivity : BaseActivity {
        
        private Dictionary<DifficultyLevel, int> m_triggersInCircleByDifficultyLevel = new() {
            { DifficultyLevel.Łatwy, 6 },
            { DifficultyLevel.Średni, 12 },
            { DifficultyLevel.Trudny, 18 }
        };
        
        private Dictionary<DifficultyLevel, double> m_directionChangeChanceByDifficultyLevel = new() {
            { DifficultyLevel.Łatwy, 0.00 },
            { DifficultyLevel.Średni, 0.05 },
            { DifficultyLevel.Trudny, 0.15 }
        };
        
        private float m_triggerCreationRadius = 0.142f;
        
        private int m_fullCircles = 3;
        private int m_triggersInCircle = 12;
        private int m_triggersCreated = 0;
        private int m_triggersCounter = 0;
        
        private bool m_isClockwiseDirection = false;
        private double m_directionChangeChance = 0.00;
        
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
                return;
            }
            
            var angle = 2 * Mathf.PI * (m_triggersCounter % m_triggersInCircle) / m_triggersInCircle;
            var x = m_triggerCreationRadius * Mathf.Cos(angle);
            var z = m_triggerCreationRadius * Mathf.Sin(angle);
            var position  = new Vector3(salad.transform.position.x + x, -0.234f, salad.transform.position.z + z);;
            if (m_isClockwiseDirection) {
                m_triggersCounter++;
            }
            else {
                m_triggersCounter--;
            }

            if (Random.value < m_directionChangeChance) {
                m_isClockwiseDirection = !m_isClockwiseDirection;
            }

            var trigger = GameObject.Instantiate(saladTrigerPrefab, position, new Quaternion(0,0,0,1));
            trigger.name = m_triggersCreated + 1 == m_triggersInCircle ? "FinalSaladTrigger" : $"SaladTrigger_{m_triggersCreated + 1}";
            trigger.GetComponent<SaladTriggerMonoBehaviour>().SetSaladActivity(this);
            
            m_triggersCreated++;
        }
        
        protected override void LoadEasy() {
            m_triggersInCircle = m_triggersInCircleByDifficultyLevel[DifficultyLevel.Łatwy];
            m_directionChangeChance = m_directionChangeChanceByDifficultyLevel[DifficultyLevel.Łatwy];
            CreateNextTrigger();
        }

        protected override void LoadMedium() {
            m_triggersInCircle = m_triggersInCircleByDifficultyLevel[DifficultyLevel.Średni];
            m_directionChangeChance = m_directionChangeChanceByDifficultyLevel[DifficultyLevel.Średni];
            CreateNextTrigger();
        }

        protected override void LoadHard() {
            m_triggersInCircle = m_triggersInCircleByDifficultyLevel[DifficultyLevel.Trudny];
            m_directionChangeChance = m_directionChangeChanceByDifficultyLevel[DifficultyLevel.Trudny];
            CreateNextTrigger();
        }
        
        protected override void SetupBodySide(BodySide bodySide) {
            switch (bodySide) {
                case BodySide.Lewa:
                    spoon.transform.localPosition = new Vector3(-0.00899999961f,-0.319999993f,-1.21099997f);
                    m_isClockwiseDirection = false;
                    break;
                case BodySide.Prawa:
                    spoon.transform.localPosition = new Vector3(-0.683000028f,-0.319999993f,-1.21099997f);
                    m_isClockwiseDirection = true;
                    break;
                default:
                    throw new InvalidOperationException("Invalid body side.");
            }
        }
    }
}