// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery.DataAccess.DataModels {
    internal class Room {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Activity> Activities { get; set; }
    }
}