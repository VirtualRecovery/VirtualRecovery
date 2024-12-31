// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

namespace VirtualRecovery {
    internal class RoomRepository {
        private DbConnector m_dbConnector;

        public RoomRepository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            EnsureRoomsTable();
        }

        private void EnsureRoomsTable() {
            if (!m_dbConnector.TableExists("Rooms")) {
               // m_dbConnector.ExecuteNonQuery(GenerateCreateTableQuery());
            }
        }
    }
}