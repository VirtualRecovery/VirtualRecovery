// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal class Room {
        private int Id { get; set; }
        private string Name { get; set; }
        private string Description { get; set; }
        private List<Activity> Activities { get; set; }
    }
}