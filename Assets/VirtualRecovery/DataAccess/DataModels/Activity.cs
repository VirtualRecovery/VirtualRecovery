// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

namespace VirtualRecovery.DataAccess.DataModels {
    internal class Activity {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsBodySideDifferentiated{ get; set; }
    }
}