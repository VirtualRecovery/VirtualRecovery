// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 02/01/2025
//  */

using System;
using System.Collections.Generic;
using VirtualRecovery.Core;

namespace VirtualRecovery.DataAccess {
    internal class DbSchemaValidator {
        private readonly DbConnector m_dbConnector;
        private readonly string m_patientsTableName;
        private readonly string m_sessionsTableName;
        private readonly string m_activitiesTableName;
        private readonly string m_roomsTableName;
        private readonly Dictionary<string, Func<string>> m_tableCreationQueries;
        private readonly Dictionary<string, Func<string>> m_tableDeletionQueries;

        public DbSchemaValidator (DbConnector connector) {
            m_dbConnector = connector;
            var config = Configuration.Instance;
            m_patientsTableName = config.configData.database.patientTableName;
            m_sessionsTableName = config.configData.database.sessionsTableName;
            m_activitiesTableName = config.configData.database.activitiesTableName;
            m_roomsTableName = config.configData.database.roomsTableName;

            m_tableCreationQueries = new Dictionary<string, Func<string>> {
                { m_patientsTableName, GenerateCreatePatientsTableQuery },
                { m_sessionsTableName, GenerateCreateSessionsTableQuery },
                { m_activitiesTableName, GenerateCreateActivitiesTableQuery },
                { m_roomsTableName, GenerateCreateRoomsTableQuery }
            };
            
            m_tableDeletionQueries = new Dictionary<string, Func<string>> {
                { m_activitiesTableName, GenerateDropActivitiesTableQuery },
                { m_roomsTableName, GenerateDropRoomsTableQuery }
            };
        }
        
        private string GenerateCreateRoomsTableQuery() {
            return $"CREATE TABLE {m_roomsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "SceneName TEXT NOT NULL" +
                   ")";
        }
    
        private string GenerateCreateActivitiesTableQuery() {
            return $"CREATE TABLE {m_activitiesTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "RoomId INTEGER NOT NULL," +
                   "Name TEXT NOT NULL," +
                   "IsBodySideDifferentiated BOOLEAN NOT NULL," +
                   $"FOREIGN KEY(RoomId) REFERENCES {m_roomsTableName}(Id)" +
                   ")";
        }
        
        private string GenerateCreatePatientsTableQuery() {
            return $"CREATE TABLE {m_patientsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "IcdCode TEXT NOT NULL," +
                   "YearOfBirth INTEGER NOT NULL," +
                   "Gender INTEGER NOT NULL," +
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
        
        private string GenerateDropRoomsTableQuery() {
            return $"DROP TABLE IF EXISTS {m_roomsTableName}";
        }
        
        private string GenerateDropActivitiesTableQuery() {
            return $"DROP TABLE IF EXISTS {m_activitiesTableName}";
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
                // Drop tables if they exist
                foreach (var table in m_tableDeletionQueries) {
                    if (TableExists(table.Key)) {
                        m_dbConnector.ExecuteQuery(table.Value());
                    }
                }
                foreach (var table in m_tableCreationQueries) {
                    if (!TableExists(table.Key)) {
                        m_dbConnector.ExecuteQuery(table.Value());
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
        }
    }
}