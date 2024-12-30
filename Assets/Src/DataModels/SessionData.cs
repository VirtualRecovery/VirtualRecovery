// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;

namespace VirtualRecovery {
    internal class SessionData {
        private int Id { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private BodySide BodySide { get; set; }
        private DifficultyLevel DifficultyLevel { get; set; }
    }
}