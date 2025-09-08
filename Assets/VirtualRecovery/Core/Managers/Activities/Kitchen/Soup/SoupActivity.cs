// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 08/09/2025
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Soup {
    internal class SoupActivity : BaseActivity {
        
        private GameObject soupTriggers => GameObject.Find("SoupTriggers")
                                    ?? throw new InvalidOperationException("No Spoon found.");
        
        private GameObject pot => GameObject.Find("pot (1)")
                                    ?? throw new InvalidOperationException("No Pot found.");
        
        private GameObject ladle => GameObject.Find("ladle")
                                    ?? throw new InvalidOperationException("No Ladle found.");
        
        private GameObject plate => GameObject.Find("soupPlate")
                                    ?? throw new InvalidOperationException("No SoupPlate found.");

        private List<GameObject> plateSoupPortions;
        
        private int m_triggersCount = 0;
        
        public SoupActivity()
            : base(
                "SoupTrigger",
                typeof(SoupTriggerMonoBehaviour),
                new Vector3(-0.0430000015f,-1.74000001f,1.93099999f),
                new Quaternion(0,0,0,1)) {
            ladle.GetComponent<SoupTriggerMonoBehaviour>().SetSoupActivity(this);
            plateSoupPortions = new List<GameObject>();
            foreach (Transform child in plate.transform) {
                if (child.gameObject.name.StartsWith("SoupPortion")) {
                    plateSoupPortions.Add(child.gameObject);
                }
            }
        }
        
        protected override void LoadEasy() {
            soupTriggers.transform.localPosition = new Vector3(0,0.000600000028f,0);
        }

        protected override void LoadMedium() {
            soupTriggers.transform.localPosition = new Vector3(0,-0.0229000002f,0);
        }

        protected override void LoadHard() {
            soupTriggers.transform.localPosition = new Vector3(0, -0.0425999984f, 0);
        }
        
        public void SoupPoured() {
            m_triggersCount++;
            plateSoupPortions[m_triggersCount - 1].SetActive(true);
            if (m_triggersCount >= 3) {
                GameManager.Instance.EndSession();
            }
        }
        
        protected override void SetupBodySide(BodySide bodySide) {
            switch (bodySide) {
                case BodySide.Lewa:
                    pot.transform.localPosition = new Vector3(0.93599999f,-0.334500015f,1.10899997f);
                    pot.transform.localRotation = new Quaternion(0, -0.573679328f, 0, 0.819080055f);
                    
                    plate.transform.localPosition = new Vector3(1.21449995f,-0.374000013f,1.11199999f);
                    plate.transform.localRotation = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
                    
                    ladle.transform.localPosition = new Vector3(0.754999995f,-0.293000013f,1.13199997f);
                    ladle.transform.localRotation = new Quaternion(-0.950701296f,-0.152554765f,0.0431684889f,0.266515762f);
                    break;
                case BodySide.Prawa:
                    pot.transform.localPosition = new Vector3(1.08599997f,-0.319000006f,1.07500005f);
                    pot.transform.localRotation = new Quaternion(0,-0.792805195f,0,0.609475195f);
                    
                    plate.transform.localPosition = new Vector3(0.782999992f,-0.374000013f,1.11199999f);
                    plate.transform.localRotation = new Quaternion(-0.707106829f, 0, 0, 0.707106829f);
                    
                    ladle.transform.localPosition = new Vector3(1.31200004f,-0.293000013f,1.13199997f);
                    ladle.transform.localRotation = new Quaternion(-0.950701296f,-0.152554765f,0.0431684889f,0.266515762f);
                    break;
                default:
                    throw new InvalidOperationException("Invalid body side.");
            }
        }
    }
}