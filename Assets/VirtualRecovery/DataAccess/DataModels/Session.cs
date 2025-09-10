// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;

namespace VirtualRecovery.DataAccess.DataModels {
    internal class Session {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ActivityId { get; set; }
        public DateTime Date { get; set; }
        public int Time { get; set; }
        public BodySide BodySide { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}