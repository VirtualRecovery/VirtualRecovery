// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using Mono.Data.Sqlite;

namespace VirtualRecovery {
    internal class DbConnector {
        private string m_dbName = "Data Source=../../Database/VirtualRecovery.db";
        private SqliteConnection m_connection;
        
        private void CreateConnection() => m_connection = new SqliteConnection(m_dbName);
    }
}