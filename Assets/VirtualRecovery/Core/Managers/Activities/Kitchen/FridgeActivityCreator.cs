// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Tomasz Krępa
//  * Created on: 08/07/2025
//  */

namespace VirtualRecovery.Core.Managers.Activities.Kitchen {
    internal class FridgeActivityCreator : ActivityCreator {
        public override IActivity CreateActivity() {
            return new FridgeActivity();
        }
    }
}