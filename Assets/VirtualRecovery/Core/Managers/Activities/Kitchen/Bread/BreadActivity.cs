// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Bread {
    internal class BreadActivity : BaseActivity {
        
        private GameObject knife => GameObject.Find("knife")
                                           ?? throw new InvalidOperationException("No Knife found.");
        
        private GameObject breadEasy => GameObject.Find("breadWide")
                                    ?? throw new InvalidOperationException("No Wide Bread found.");
        
        private GameObject breadMedium => GameObject.Find("breadMedium")
                                        ?? throw new InvalidOperationException("No Medium Bread found.");
        
        private GameObject breadHard => GameObject.Find("breadNarrow")
                                          ?? throw new InvalidOperationException("No Narrow Bread found.");

        private GameObject m_chosenBread;

        private int interval = 1; // 1 for left hand, -1 for right hand
        
        private static readonly Vector3 s_breadPosition = new Vector3(0.954874456f,-0.245000005f,-1.16199994f);
        private static readonly Quaternion s_breadRotation = new Quaternion(0,0,0,1);
        
        private int m_slicesToCut;
        private int m_slicesCut = 0;
        
        private float m_triggersOffsetLeft = 0;
        
        private List<GameObject> breadTriggers = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.name.StartsWith("BreadTrigger"))
            .ToList();

        public BreadActivity()
            : base(
                "Tylna ściana",
                typeof(BreadTriggerMonoBehaviour),
                new Vector3(-0.143999994f,-1.64999998f,0.763000011f),
                new Quaternion(0, -1, 0, 0)) {
        }
        
        public void SliceCut() {
            m_slicesCut++;
            if (m_slicesCut >= m_slicesToCut) {
                GameManager.Instance.EndSession();
                return;
            }
            
            if (interval == 1) {
                ActivateTrigger(m_slicesToCut + 1 - m_slicesCut);
            }
            else {
                ActivateTrigger(m_slicesCut + 1);
            }
        }
        
        protected override void LoadEasy() {
            m_chosenBread = breadEasy;
            breadEasy.transform.localPosition = s_breadPosition;
            breadEasy.transform.localRotation = s_breadRotation;
            breadEasy.GetComponent<Rigidbody>().useGravity = true;
            m_slicesToCut = 3;
        }

        protected override void LoadMedium() {
            m_chosenBread = breadMedium;
            breadMedium.transform.localPosition = s_breadPosition;
            breadMedium.transform.localRotation = s_breadRotation;
            breadMedium.GetComponent<Rigidbody>().useGravity = true;
            m_slicesToCut = 7;
        }

        protected override void LoadHard() {
            m_chosenBread = breadHard;
            breadHard.transform.localPosition = s_breadPosition;
            breadHard.transform.localRotation = s_breadRotation;
            breadHard.GetComponent<Rigidbody>().useGravity = true;
            m_slicesToCut = 15;
        }

        protected override void SetupBodySide(BodySide bodySide) {
            switch (bodySide) {
                case BodySide.Lewa:
                    knife.transform.localPosition = new Vector3(1.31500006f,-0.296000004f,-1.19305933f);
                    knife.transform.localRotation = new Quaternion(0,-0.707106829f,0,0.707106829f);
                    interval = 1;
                    m_triggersOffsetLeft = CalculateOffset();
                    ActivateTrigger(m_slicesToCut + 1);
                    break;
                case BodySide.Prawa:
                    knife.transform.localPosition = new Vector3(0.550000012f,-0.296000004f,-1.19305933f);
                    knife.transform.localRotation = new Quaternion(0,-0.707106829f,0,0.707106829f);
                    interval = -1;
                    m_triggersOffsetLeft = CalculateOffset();
                    ActivateTrigger(1);
                    break;
                default:
                    throw new InvalidOperationException("Invalid body side.");
            }
        }
        
        private void ActivateTrigger(int sliceNumber) {
            var trigger = m_chosenBread.transform.Find("slice" + sliceNumber)?.transform.Find("BreadTrigger");
            if (trigger != null) {
                trigger.gameObject.SetActive(true);
                if (interval == 1 && sliceNumber != m_slicesToCut + 1) {
                    trigger.transform.position = new Vector3(trigger.transform.position.x - m_triggersOffsetLeft, trigger.transform.position.y, trigger.transform.position.z);
                }
                trigger.GetComponent<BreadTriggerMonoBehaviour>().SetBreadActivity(this);
            }
        }

        private float CalculateOffset() {
            var trigger1 = m_chosenBread.transform.Find("slice" + (m_slicesToCut + 1))?.transform.Find("BreadTrigger");
            var trigger2 = m_chosenBread.transform.Find("slice" + (m_slicesToCut - 1))?.transform.Find("BreadTrigger");
            float temp = 0;
            if (trigger1 != null && trigger2 != null) {
                if (interval == 1) {
                    temp = trigger1.position.x - trigger2.position.x;
                }
                trigger1.gameObject.SetActive(false);
                trigger2.gameObject.SetActive(false);
            }
            return temp;
        }
    }
}