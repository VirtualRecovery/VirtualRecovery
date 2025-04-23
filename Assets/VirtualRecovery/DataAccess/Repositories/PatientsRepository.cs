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
        
        private List<Session> GetSessionsForPatient(int patientId) {
            var sessions = new List<Session>();
            var sessionsQuery = $"SELECT * FROM {m_sessionsTableName} WHERE PatientId = @PatientId";
            
            using (var sessionsCommand = m_dbConnector.CreateCommand(sessionsQuery, ("@PatientId", patientId))) {
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
                            IcdCode = reader.GetString(1),
                            YearOfBirth = reader.GetInt32(2),
                            Gender = (Gender)reader.GetInt32(3),
                            WeakBodySide = (BodySide)reader.GetInt32(4),
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
                using (var command = m_dbConnector.CreateCommand(query)) {
                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var patient = new Patient {
                                Id = reader.GetInt32(0),
                                IcdCode = reader.GetString(1),
                                YearOfBirth = reader.GetInt32(2),
                                Gender = (Gender)reader.GetInt32(3),
                                WeakBodySide = (BodySide)reader.GetInt32(4),
                                SessionsHistory = new List<Session>()
                            };
                            patients.Add(patient);
                        }
                    }
                }

                foreach (var patient in patients) {
                    patient.SessionsHistory = GetSessionsForPatient(patient.Id);
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return patients;
        }
    }
}