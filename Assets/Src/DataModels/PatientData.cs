// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientData {
        // TODO: Ask what Id should we use. (Id of EDM? PESEL?)
        private string Name { get; set; }
        private string Surname { get; set; }
        private BodySide WeakBodySide { get; set; }
        private List<SessionData> SessionsHistory { get; set; }
    }
}