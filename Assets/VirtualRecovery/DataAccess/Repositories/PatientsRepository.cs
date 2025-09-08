// /*
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
            var query = $"INSERT INTO {m_patientsTableName} (IcdCode, YearOfBirth, Gender, WeakBodySide)" +
                        "VALUES (@IcdCode, @YearOfBirth, @Gender, @WeakBodySide)";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@IcdCode", entity.IcdCode),
                           ("@YearOfBirth", entity.YearOfBirth),
                           ("@Gender", (int)entity.Gender),
                           ("@WeakBodySide", (int)entity.WeakBodySide))) {
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
            var query = $"UPDATE {m_patientsTableName} SET IcdCode = @IcdCode, YearOfBirth = @YearOfBirth, " +
                        "Gender = @Gender, WeakBodySide = @WeakBodySide, SessionsHistory = @SessionsHistory WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@IcdCode", entity.IcdCode),
                           ("@YearOfBirth", entity.YearOfBirth),
                           ("@Gender", (int)entity.Gender),
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
        
        public List<Session> GetSessionsForPatient(int patientId) {
            var sessions = new List<Session>();
            var sessionsQuery = $"SELECT * FROM {m_sessionsTableName} WHERE PatientId = @PatientId";
            
            m_dbConnector.OpenConnection();
            try {
                using (var sessionsCommand = m_dbConnector.CreateCommand(sessionsQuery, ("@PatientId", patientId))) {
                    using (var sessionsReader = sessionsCommand.ExecuteReader()) {
                        while (sessionsReader.Read()) {
                            var session = new Session {
                                Id = sessionsReader.GetInt32(0),
                                PatientId = sessionsReader.GetInt32(1),
                                ActivityId = sessionsReader.GetInt32(2),
                                Date = sessionsReader.GetDateTime(3),
                                Time = sessionsReader.GetInt32(4),
                                BodySide = (BodySide)sessionsReader.GetInt32(5),
                                DifficultyLevel = (DifficultyLevel)sessionsReader.GetInt32(6)
                            };
                            sessions.Add(session);
                        }
                    }
                }
            } finally {
                m_dbConnector.CloseConnection();
            }
            return sessions;
        }
        
        public void InsertSessionForPatient(int patientId, Session session) {
            var query = $"INSERT INTO {m_sessionsTableName} (PatientId, ActivityId, Date, Time, BodySide, DifficultyLevel) " +
                        "VALUES (@PatientId, @ActivityId, @Date, @Time, @BodySide, @DifficultyLevel)";

            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@PatientId", patientId),
                           ("@ActivityId", session.ActivityId),
                           ("@Date", session.Date.ToString("yyyy-MM-dd")),
                           ("@Time", session.Time),
                           ("@BodySide", (int)session.BodySide),
                           ("@DifficultyLevel", (int)session.DifficultyLevel))) {
                    if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                        throw new Exception("No rows were updated.");
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
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
                            IcdCode = reader.GetString(1),
                            YearOfBirth = reader.GetInt32(2),
                            Gender = (Gender)reader.GetInt32(3),
                            WeakBodySide = (BodySide)reader.GetInt32(4)
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
                using (var command = m_dbConnector.CreateCommand(query)) {
                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var patient = new Patient {
                                Id = reader.GetInt32(0),
                                IcdCode = reader.GetString(1),
                                YearOfBirth = reader.GetInt32(2),
                                Gender = (Gender)reader.GetInt32(3),
                                WeakBodySide = (BodySide)reader.GetInt32(4),
                            };
                            patients.Add(patient);
                        }
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