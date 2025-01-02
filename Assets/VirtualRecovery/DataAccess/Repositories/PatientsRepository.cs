﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;
using VirtualRecovery.Core;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.DataAccess.Repositories {
    internal class PatientsRepository : IRepository<Patient> {
        private readonly DbConnector m_dbConnector;
        private readonly string m_patientsTableName;
        private readonly string m_sessionsTableName;

        public PatientsRepository() {
            m_dbConnector = new DbConnector();
            
            var config = Configuration.Instance;
            m_patientsTableName = config.configData.database.patientTableName;
            m_sessionsTableName = config.configData.database.sessionsTableName;
        }
        
        public void Insert(Patient entity) {
            var query = $"INSERT INTO {m_patientsTableName} (Name, Surname, WeakBodySide, SessionsHistory)" +
                        "VALUES (@Name, @Surname, @WeakBodySide, @SessionsHistory)";
            
            m_dbConnector.OpenConnection();
            try {
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
            finally {
                m_dbConnector.CloseConnection();
            }
        }

        public void Update(int id, Patient entity) {
            var query = $"UPDATE {m_patientsTableName} SET Name = @Name, Surname = @Surname, " +
                        "WeakBodySide = @WeakBodySide, SessionsHistory = @SessionsHistory WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
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
            finally {
                m_dbConnector.CloseConnection();
            }
        }

        public void Delete(int id) {
            var query = $"DELETE FROM {m_patientsTableName} WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query, ("@Id", id))) {
                    if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                        throw new Exception("No rows were updated.");
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
        }
        
        private List<Session> GetSessionsForPatient(int patientId) {
            var sessions = new List<Session>();
            var sessionsQuery = $"SELECT * FROM {m_sessionsTableName} WHERE PatientId = @PatientId";
            
            m_dbConnector.OpenConnection();
            try {
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
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return sessions;
        }

        public Patient GetById(int id) {
            var query = $"SELECT * FROM {m_patientsTableName} WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
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
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return null;
        }

        public List<Patient> GetAll() {
            var query = $"SELECT * FROM {m_patientsTableName}";
            var patients = new List<Patient>();
    
            m_dbConnector.OpenConnection();
            try {
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
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return patients;
        }
    }
}