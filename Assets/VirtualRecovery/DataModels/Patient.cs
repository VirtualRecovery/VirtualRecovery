// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal class Patient {
        // TODO: Ask what Id should we use. (Id of EDM? PESEL?)
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BodySide WeakBodySide { get; set; }
        public List<Session> SessionsHistory { get; set; }
    }
}