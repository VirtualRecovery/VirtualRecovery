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

        private static readonly Vector3 s_leftPotPosition = new Vector3(-0.5f, -0.334500015f, 1.07799995f);
        private static readonly Vector3 s_rightPotPosition = new Vector3(0.532000005f,-0.334500015f,1.07799995f);

        private static readonly Vector3 s_leftPlayerStartPosition =
            new Vector3(1.79700005f, 0.364356041f, -1.26800001f);
        private static readonly Vector3 s_rightPlayerStartPosition =
            new Vector3(2.26399994f,0.364356041f,-1.26800001f);
        
        private bool m_flipToOppositeSide = false;
        private string m_upOrDown = "Down";
        private string m_leftOrRight = "Left";
        
        public StoveActivity() 
            : base(
                "", 
                typeof(StoveTriggerMonoBehaviour), 
                new Vector3(1.79700005f,0.364356041f,-1.26800001f), 
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
                    m_playerStartPosition = s_leftPlayerStartPosition;
                    m_leftOrRight = m_flipToOppositeSide ? "Left" : "Right";
                    break;
                case BodySide.Prawa:
                    m_playerStartPosition = s_rightPlayerStartPosition;
                    m_leftOrRight = m_flipToOppositeSide ? "Right" : "Left";
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