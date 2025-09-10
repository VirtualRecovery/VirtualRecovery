// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Stove {
    internal class StoveActivity : BaseActivity {
        
        private GameObject pot => GameObject.Find("pot")
                                      ?? throw new InvalidOperationException("No Pot found.");
        
        private bool m_flipToOppositeSide = false;
        private string m_upOrDown = "Down";
        private string m_leftOrRight = "Left";
        
        public StoveActivity() 
            : base(
                "", 
                typeof(StoveTriggerMonoBehaviour), 
                new Vector3(-1.10699999f,-1.74000001f,1.93099999f), 
                new Quaternion(0,0,0,1)) 
        { }


        protected override void LoadEasy() {
            m_upOrDown = "Down";
            m_flipToOppositeSide = false;
        }

        protected override void LoadMedium() {
            m_upOrDown = "Up";
            m_flipToOppositeSide = false;
        }

        protected override void LoadHard() {
            m_upOrDown = "Up";
            m_flipToOppositeSide = true;
        }

        protected override void SetupBodySide(BodySide bodySide) {
            switch (bodySide) {
                case BodySide.Lewa:
                    pot.transform.position = new Vector3(-0.463999987f, -0.334500015f, 1.18200004f);
                    m_leftOrRight = m_flipToOppositeSide ? "Right" : "Left";
                    break;
                case BodySide.Prawa:
                    pot.transform.position = new Vector3(0.442000002f,-0.334500015f,1.18200004f);
                    m_leftOrRight = m_flipToOppositeSide ? "Left" : "Right";
                    break;
            }

            m_sessionEndingObjectName = m_leftOrRight + m_upOrDown + "Burner";
            var burner = FindBurner(m_sessionEndingObjectName);
            if (burner == null) {
                throw new InvalidOperationException($"No {m_sessionEndingObjectName} found.");
            }
            burner.SetActive(true);
        }

        private GameObject FindBurner(string burnerName) {
            var burners = GameObject.Find("Burners");
            foreach (Transform burner in burners.transform) {
                if (burner.name == burnerName) {
                    return burner.gameObject;
                }
            }
            return null;
        }

    }
}