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
    internal class FridgeActivity : BaseActivity {
        
        private const float k_easyLevelHeight = -0.00405f;
        private const float k_mediumLevelHeight = 0.00059f;
        private const float k_hardLevelHeight = 0.00417f;

        private const float k_leftHingeLimitMin = 0;
        private const float k_leftHingeLimitMax = 90;
        private static readonly Vector3 s_leftHingeAnchor = new Vector3(0f, -0.00600000005f, 0f);
        private const float k_leftHandleY = -0.00065f;
        
        private const float k_rightHingeLimitMin = -90;
        private const float k_rightHingeLimitMax = 0;
        private static readonly Vector3 s_rightHingeAnchor = new Vector3(0f,1.86264515e-09f,0f);
        private const float k_rightHandleY = -0.00539f;

        private static readonly Vector3 s_doorDefaultPosition = new Vector3(-0.25f, 0.380690038f, -0.300000012f);

        private GameObject fridgeHandle => GameObject.Find("Uchwyt góra")
                                           ?? throw new InvalidOperationException("No Fridge Handle found.");
        
        public FridgeActivity() 
            : base(
                "FridgeOpenTriggerCollider", 
                typeof(FridgeTriggerMonoBehaviour), 
                new Vector3(-0.0909999982f,-1.47000003f,-1.63600004f), 
                new Quaternion(0,-1,0,0)) 
        { }
        
        private void ChangeHandleHeight(float height) {
            var connectedBody = fridgeHandle.GetComponent<FixedJoint>().connectedBody;
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = null;
            fridgeHandle.transform.localPosition  = new Vector3(fridgeHandle.transform.localPosition .x, 
                fridgeHandle.transform.localPosition .y, height);
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = connectedBody;
        }
        
        protected override void LoadEasy() {
            ChangeHandleHeight(k_easyLevelHeight);
        }

        protected override void LoadMedium() {
            ChangeHandleHeight(k_mediumLevelHeight);
        }

        protected override void LoadHard() {
            ChangeHandleHeight(k_hardLevelHeight);
        }

        private struct BodySideConfig {
            public readonly float HingeLimitMin;
            public readonly float HingeLimitMax;
            public readonly Vector3 HingeAnchor;
            public readonly float HandleY;

            public BodySideConfig(
                float hingeLimitMin,
                float hingeLimitMax,
                Vector3 hingeAnchor,
                float handleY)
            {
                HingeLimitMin = hingeLimitMin;
                HingeLimitMax = hingeLimitMax;
                HingeAnchor   = hingeAnchor;
                HandleY       = handleY;
            }
        }

        private readonly Dictionary<BodySide, BodySideConfig> m_bodySideConfigs = new() {
            {
                BodySide.Lewa, new BodySideConfig(
                    hingeLimitMin: k_leftHingeLimitMin,
                    hingeLimitMax: k_leftHingeLimitMax,
                    hingeAnchor: s_leftHingeAnchor,
                    handleY: k_leftHandleY
                )
            }, {
                BodySide.Prawa, new BodySideConfig(
                    hingeLimitMin: k_rightHingeLimitMin,
                    hingeLimitMax: k_rightHingeLimitMax,
                    hingeAnchor: s_rightHingeAnchor,
                    handleY: k_rightHandleY
                )
            }
        };

        protected override void SetupBodySide(BodySide bodySide) {
            GameObject fridgeDoor = GameObject.Find("fridgeDoor")
                ?? throw new InvalidOperationException("No Fridge Door found.");

            if(!m_bodySideConfigs.TryGetValue(bodySide, out var bodySideConfig))
                throw new ArgumentException($"Unknown body side: {bodySide}.");
            
            fridgeDoor.transform.localPosition = s_doorDefaultPosition;
            
            var doorHinge = fridgeDoor.GetComponent<HingeJoint>();
            
            var limits = doorHinge.limits;
            limits.min = bodySideConfig.HingeLimitMin;
            limits.max = bodySideConfig.HingeLimitMax;
            doorHinge.limits = limits;
            doorHinge.anchor = bodySideConfig.HingeAnchor;

            var connectedBody = fridgeHandle.GetComponent<FixedJoint>().connectedBody;
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = null;
            fridgeHandle.transform.localPosition  = new Vector3(fridgeHandle.transform.localPosition .x, 
                bodySideConfig.HandleY, fridgeHandle.transform.localPosition .z);
            fridgeHandle.GetComponent<FixedJoint>().connectedBody = connectedBody;

        }
    }
}