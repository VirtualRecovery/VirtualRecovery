// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

namespace VirtualRecovery.DataAccess.DataModels {
    [System.Serializable]
    internal class Activity {
        public int Id;
        public int RoomId;
        public string Name;
        public bool IsBodySideDifferentiated;
    }

}