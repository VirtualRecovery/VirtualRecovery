// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientRepository : IRepository<PatientData> {
        private readonly DbConnector m_dbConnector;
        private const string k_patientTableName = "Patients";

        public PatientRepository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            EnsureTable();
        }
        
        private string GenerateCreateTableQuery() {
            return "CREATE TABLE Patients (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Surname TEXT NOT NULL," +
                   "WeakBodySide INTEGER NOT NULL," +
                   "SessionsHistory TEXT NOT NULL" +
                   ")";
        }

        private void EnsureTable() {
            if (!m_dbConnector.TableExists(k_patientTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreateTableQuery());
            }
        }
        
        public void Insert(PatientData entity) {
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

        public void Update(int id, PatientData entity) {
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

        public PatientData GetById(int id) {
            /*var reader = m_dbConnector.ExecuteReader($"SELECT * FROM {k_patientTableName} WHERE Id = {id}");
            if (!reader.Read()) {
                return null;
            }

            return new PatientData {
                Name = reader.GetString(1),
                Surname = reader.GetString(2),
                WeakBodySide = (BodySide)reader.GetInt32(3),
                SessionsHistory = reader.GetString(4)
            };*/
            throw new NotImplementedException();
        }

        public List<PatientData> GetAll() {
            /*var reader = m_dbConnector.ExecuteReader($"SELECT * FROM {k_patientTableName}");
            var patients = new List<PatientData>();
            while (reader.Read()) {
                patients.Add(new PatientData {
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    WeakBodySide = (BodySide)reader.GetInt32(3),
                    SessionsHistory = reader.GetString(4)
                });
            }

            return patients;*/
            throw new NotImplementedException();
        }
    }
}