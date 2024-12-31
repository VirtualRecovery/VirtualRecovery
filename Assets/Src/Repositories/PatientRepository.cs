﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientRepository : IRepository<Patient> {
        private readonly DbConnector m_dbConnector;
        private const string k_patientTableName = "Patients";
        private const string k_sessionsTableName = "Sessions";

        public PatientRepository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            EnsureTables();
        }

        private string GenerateCreatePatientsTableQuery() {
            return $"CREATE TABLE {k_patientTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Surname TEXT NOT NULL," +
                   "WeakBodySide INTEGER NOT NULL" +
                   ")";
        }

        private string GenerateCreateSessionsTableQuery() {
            return $"CREATE TABLE {k_sessionsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "PatientId INTEGER NOT NULL," +
                   "StartDate TEXT NOT NULL," +
                   "EndDate TEXT NOT NULL," +
                   "BodySide INTEGER NOT NULL," +
                   "DifficultyLevel INTEGER NOT NULL," +
                   $"FOREIGN KEY(PatientId) REFERENCES {k_patientTableName}(Id)" +
                   ")";
        }

        private void EnsureTables() {
            if (!m_dbConnector.TableExists(k_patientTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreatePatientsTableQuery());
            }
            
            if (!m_dbConnector.TableExists(k_sessionsTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreateSessionsTableQuery());
            }
        }
        
        public void Insert(Patient entity) {
            var query = $"INSERT INTO {k_patientTableName} (Name, Surname, WeakBodySide, SessionsHistory)" +
                        "VALUES (@Name, @Surname, @WeakBodySide, @SessionsHistory)";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.Name),
                       ("@Surname", entity.Surname),
                       ("@WeakBodySide", (int)entity.WeakBodySide),
                       ("@SessionsHistory", entity.SessionsHistory))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Update(int id, Patient entity) {
            var query = $"UPDATE {k_patientTableName} SET Name = @Name, Surname = @Surname, " +
                        "WeakBodySide = @WeakBodySide, SessionsHistory = @SessionsHistory WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.Name),
                       ("@Surname", entity.Surname),
                       ("@WeakBodySide", (int)entity.WeakBodySide),
                       ("@SessionsHistory", entity.SessionsHistory),
                       ("@Id", id))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Delete(int id) {
            var query = $"DELETE FROM {k_patientTableName} WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query, ("@Id", id))) {
                if(m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }
        
        private List<Session> GetSessionsForPatient(int patientId) {
            var sessions = new List<Session>();
            var sessionsQuery = $"SELECT * FROM {k_sessionsTableName} WHERE PatientId = @PatientId";
    
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
            var query = $"SELECT * FROM {k_patientTableName} WHERE Id = @Id";
    
            using (var command = m_dbConnector.CreateCommand(query, ("@Id", id)))
            using (var reader = command.ExecuteReader()) {
                if (reader.Read()) {
                    var patient = new Patient {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        WeakBodySide = (BodySide)reader.GetInt32(3),
                        SessionsHistory = GetSessionsForPatient(id)
                    };
            
                    return patient;
                }
            }

            return null;
        }

        public List<Patient> GetAll() {
            var query = $"SELECT * FROM {k_patientTableName}";
            var patients = new List<Patient>();
    
            using (var command = m_dbConnector.CreateCommand(query))
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var patient = new Patient {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
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