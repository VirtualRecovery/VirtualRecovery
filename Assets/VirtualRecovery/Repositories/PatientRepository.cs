// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientRepository : IRepository<Patient> {
        private readonly DbConnector m_dbConnector;
        private readonly string m_patientTableName;
        private readonly string m_sessionsTableName;
        private readonly string m_activitiesTableName;

        public PatientRepository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            
            var config = Configuration.Instance;
            m_patientTableName = config.configData.database.patientTableName;
            m_sessionsTableName = config.configData.database.sessionsTableName;
            m_activitiesTableName = config.configData.database.activitiesTableName;
            
            EnsureTables();
        }

        private string GenerateCreatePatientsTableQuery() {
            return $"CREATE TABLE {m_patientTableName} (" +
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
                   $"FOREIGN KEY(PatientId) REFERENCES {m_patientTableName}(Id)," +
                   $"FOREIGN KEY(ActivityId) REFERENCES {m_activitiesTableName}(Id)" +
                   ")";
        }

        private void EnsureTables() {
            if (!m_dbConnector.TableExists(m_patientTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreatePatientsTableQuery());
            }
            
            if (!m_dbConnector.TableExists(m_sessionsTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreateSessionsTableQuery());
            }
        }
        
        public void Insert(Patient entity) {
            var query = $"INSERT INTO {m_patientTableName} (Name, Surname, WeakBodySide, SessionsHistory)" +
                        "VALUES (@Name, @Surname, @WeakBodySide, @SessionsHistory)";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.FirstName),
                       ("@Surname", entity.LastName),
                       ("@WeakBodySide", (int)entity.WeakBodySide),
                       ("@SessionsHistory", entity.SessionsHistory))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Update(int id, Patient entity) {
            var query = $"UPDATE {m_patientTableName} SET Name = @Name, Surname = @Surname, " +
                        "WeakBodySide = @WeakBodySide, SessionsHistory = @SessionsHistory WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.FirstName),
                       ("@Surname", entity.LastName),
                       ("@WeakBodySide", (int)entity.WeakBodySide),
                       ("@SessionsHistory", entity.SessionsHistory),
                       ("@Id", id))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Delete(int id) {
            var query = $"DELETE FROM {m_patientTableName} WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query, ("@Id", id))) {
                if(m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }
        
        private List<Session> GetSessionsForPatient(int patientId) {
            var sessions = new List<Session>();
            var sessionsQuery = $"SELECT * FROM {m_sessionsTableName} WHERE PatientId = @PatientId";
    
            using (var sessionsCommand = m_dbConnector.CreateCommand(sessionsQuery, ("@PatientId", patientId)))
            using (var sessionsReader = sessionsCommand.ExecuteReader()) {
                while (sessionsReader.Read()) {
                    var session = new Session {
                        Id = sessionsReader.GetInt32(0),
                        PatientId = sessionsReader.GetInt32(1),
                        StartDate = sessionsReader.GetDateTime(2),
                        EndDate = sessionsReader.GetDateTime(3),
                        BodySide = (BodySide)sessionsReader.GetInt32(4),
                        DifficultyLevel = (DifficultyLevel)sessionsReader.GetInt32(5)
                    };
                    sessions.Add(session);
                }
            }

            return sessions;
        }

        public Patient GetById(int id) {
            var query = $"SELECT * FROM {m_patientTableName} WHERE Id = @Id";
    
            using (var command = m_dbConnector.CreateCommand(query, ("@Id", id)))
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    var patient = new Patient {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WeakBodySide = (BodySide)reader.GetInt32(3),
                        SessionsHistory = GetSessionsForPatient(id)
                    };
            
                    return patient;
                }
            }

            return null;
        }

        public List<Patient> GetAll() {
            var query = $"SELECT * FROM {m_patientTableName}";
            var patients = new List<Patient>();
    
            using (var command = m_dbConnector.CreateCommand(query))
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var patient = new Patient {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WeakBodySide = (BodySide)reader.GetInt32(3),
                        SessionsHistory = GetSessionsForPatient(reader.GetInt32(0))
                    };
            
                    patients.Add(patient);
                }
            }

            return patients;
        }
    }
}