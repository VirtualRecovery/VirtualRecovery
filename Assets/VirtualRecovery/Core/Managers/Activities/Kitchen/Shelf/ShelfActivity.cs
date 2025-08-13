// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

using System;
using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf {
    internal class ShelfActivity : BaseActivity {
        
        private const float k_easyLevelHeight = -0.05f;
        private const float k_mediumLevelHeight = 0.05f;
        private const float k_hardLevelHeight = 0.2f;
        
        public ShelfActivity() 
            : base(
                "shelf", 
                typeof(ShelfTriggerMonoBehaviour), 
                new Vector3(-1.96000004f,-1.64999998f,1.78999996f), 
                new Quaternion(0,0,0,1)) 
        { }

        private void ChangeShelfHeight(float height) {
            var shelf = GameObject.Find("shelf")
                        ?? throw new InvalidOperationException("No shelf found.");
            
            shelf.transform.position = new Vector3(shelf.transform.position.x, height, shelf.transform.position.z);
        }

        protected override void LoadEasy() {
            ChangeShelfHeight(k_easyLevelHeight);
        }

        protected override void LoadMedium() {
            ChangeShelfHeight(k_mediumLevelHeight);
        }

        protected override void LoadHard() {
            ChangeShelfHeight(k_hardLevelHeight);
        }

        protected override void SetupBodySide(BodySide bodySide) {
            return;
        }
    }
}