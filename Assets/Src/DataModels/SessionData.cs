﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;

namespace VirtualRecovery {
    internal class SessionData {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BodySide BodySide { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}