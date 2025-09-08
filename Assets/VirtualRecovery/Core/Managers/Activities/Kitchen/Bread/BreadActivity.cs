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
        
        private static readonly Vector3 s_breadPosition = new Vector3(0.954874456f,-0.245000005f,-1.16199994f);
        private static readonly Quaternion s_breadRotation = new Quaternion(0,0,0,1);
        
        private int m_slicesToCut;
        private int m_slicesCut = 0;
        
        private List<GameObject> breadTriggers = GameObject.FindObjectsOfType<GameObject>()
            .Where(go => go.name.StartsWith("BreadTrigger"))
            .ToList();

        public BreadActivity()
            : base(
                "Tylna ściana",
                typeof(BreadTriggerMonoBehaviour),
                new Vector3(2.01900005f, -1.64999998f, -1.84099996f),
                new Quaternion(0, -1, 0, 0)) {
            foreach (var breadTrigger in breadTriggers) {
                breadTrigger.GetComponent<BreadTriggerMonoBehaviour>().SetBreadActivity(this);
            }
        }
        
        public void SliceCut() {
            m_slicesCut++;
            if (m_slicesCut >= m_slicesToCut) {
                GameManager.Instance.EndSession();
            }
        }
        
        protected override void LoadEasy() {
            breadEasy.transform.localPosition = s_breadPosition;
            breadEasy.transform.localRotation = s_breadRotation;
            breadEasy.GetComponent<Rigidbody>().useGravity = true;
            m_slicesToCut = 3;
        }

        protected override void LoadMedium() {
            breadMedium.transform.localPosition = s_breadPosition;
            breadMedium.transform.localRotation = s_breadRotation;
            breadMedium.GetComponent<Rigidbody>().useGravity = true;
            m_slicesToCut = 7;
        }

        protected override void LoadHard() {
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
                    break;
                case BodySide.Prawa:
                    knife.transform.localPosition = new Vector3(0.550000012f,-0.296000004f,-1.19305933f);
                    knife.transform.localRotation = new Quaternion(0,-0.707106829f,0,0.707106829f);
                    break;
                default:
                    throw new InvalidOperationException("Invalid body side.");
            }
        }
    }
}