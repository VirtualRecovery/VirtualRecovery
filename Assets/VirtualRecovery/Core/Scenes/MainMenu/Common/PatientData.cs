// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 23/04/2025
//  */

using UnityEngine;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Scenes.MainMenu.Common {
    // used to store patient data under UI elements
    internal class PatientData : MonoBehaviour {
        public Patient patient { get; set; }
    }
}