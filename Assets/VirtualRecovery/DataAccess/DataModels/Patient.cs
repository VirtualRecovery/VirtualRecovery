// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery.DataAccess.DataModels {
    internal class Patient {
        public int Id { get; set; }
        public string IcdCode { get; set; }
        public int YearOfBirth { get; set; }
        public Gender Gender { get; set; }
        public BodySide WeakBodySide { get; set; }
        public List<Session> SessionsHistory { get; set; }
    }
}