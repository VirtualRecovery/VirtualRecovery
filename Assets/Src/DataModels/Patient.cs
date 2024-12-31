// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientData {
        // TODO: Ask what Id should we use. (Id of EDM? PESEL?)
        public string Name { get; set; }
        public string Surname { get; set; }
        public BodySide WeakBodySide { get; set; }
        public List<SessionData> SessionsHistory { get; set; }
    }
}