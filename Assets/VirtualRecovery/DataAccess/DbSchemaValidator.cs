// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 02/01/2025
//  */

namespace VirtualRecovery.DataAccess {
    internal class DbSchemaValidator {
        private readonly string m_roomsTableName;
        private readonly string m_activitiesTableName;
        private readonly string m_sessionsTableName;
        private readonly string m_patientsTableName;
        private readonly DbConnector m_dbConnector;

        public DbSchemaValidator (DbConnector connector) {
            m_dbConnector = connector;
        }
        
        private string GenerateCreateRoomsTableQuery() {
            return $"CREATE TABLE {m_roomsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Description TEXT NOT NULL" +
                   ")";
        }
    
        private string GenerateCreateActivitiesTableQuery() {
            return $"CREATE TABLE {m_activitiesTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "RoomId INTEGER NOT NULL," +
                   "Name TEXT NOT NULL," +
                   "Description TEXT NOT NULL," +
                   "IsBodySideDifferentiated BOOLEAN NOT NULL," +
                   $"FOREIGN KEY(RoomId) REFERENCES {m_roomsTableName}(Id)" +
                   ")";
        }
        
        private string GenerateCreatePatientsTableQuery() {
            return $"CREATE TABLE {m_patientsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Surname TEXT NOT NULL," +
                   "WeakBodySide INTEGER NOT NULL" +
                   ")";
        }
        
        private string GenerateCreateSessionsTableQuery() {
            return $"CREATE TABLE {m_sessionsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "PatientId INTEGER NOT NULL," +
                   "ActivityId INTEGER NOT NULL," +
                   "StartDate TEXT NOT NULL," +
                   "EndDate TEXT NOT NULL," +
                   "BodySide INTEGER NOT NULL," +
                   "DifficultyLevel INTEGER NOT NULL," +
                   $"FOREIGN KEY(PatientId) REFERENCES {m_patientsTableName}(Id)," +
                   $"FOREIGN KEY(ActivityId) REFERENCES {m_activitiesTableName}(Id)" +
                   ")";
        }
        
        private bool TableExists(string tableName) {
            var command = m_dbConnector
                .CreateCommand("SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName;");
            
            command.Parameters.AddWithValue("@tableName", tableName);
            
            using (var reader = command.ExecuteReader()) {
                return reader.Read();
            }
        }
        
        public void EnsureTables() {
            m_dbConnector.OpenConnection();
            try {
                if (!TableExists(m_roomsTableName)) {
                    m_dbConnector.ExecuteQuery(GenerateCreateRoomsTableQuery());
                }

                if (!TableExists(m_activitiesTableName)) {
                    m_dbConnector.ExecuteQuery(GenerateCreateActivitiesTableQuery());
                }
                
                if (!TableExists(m_patientsTableName)) {
                    m_dbConnector.ExecuteQuery(GenerateCreatePatientsTableQuery());
                }

                if (!TableExists(m_sessionsTableName)) {
                    m_dbConnector.ExecuteQuery(GenerateCreateSessionsTableQuery());
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
        }
    }
}