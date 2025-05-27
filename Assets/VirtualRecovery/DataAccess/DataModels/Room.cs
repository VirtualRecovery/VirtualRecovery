// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery.DataAccess.DataModels {
    [System.Serializable]
    internal class Room {
        public int Id;
        public string Name;
        public List<Activity> Activities;
    }
}