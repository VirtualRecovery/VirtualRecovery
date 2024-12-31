// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal class PatientRepository : Repository<PatientData> {
        protected override string GetTableName() => "Patients";

        public PatientRepository() : base() { }

        protected override string GenerateCreateTableQuery() {
            return "CREATE TABLE Patients (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Surname TEXT NOT NULL," +
                   "WeakBodySide INTEGER NOT NULL," +
                   "SessionsHistory TEXT NOT NULL" +
                   ")";
        }

        public override void Insert(PatientData entity) {
            m_dbConnector.ExecuteNonQuery(
                "INSERT INTO Patients (Name, Surname, WeakBodySide, SessionsHistory) VALUES (" +
                $"'{entity.Name}', '{entity.Surname}', {(int)entity.WeakBodySide}, '{entity.SessionsHistory}'" +
                ")"
            );
        }

        public override void Update(int id, PatientData entity) {
            m_dbConnector.ExecuteNonQuery(
                "UPDATE Patients SET " +
                $"Name = '{entity.Name}', Surname = '{entity.Surname}', WeakBodySide = {(int)entity.WeakBodySide}, SessionsHistory = '{entity.SessionsHistory}'" +
                $" WHERE Id = {id}"
            );
        }

        public override void Delete(int id) {
            m_dbConnector.ExecuteNonQuery($"DELETE FROM Patients WHERE Id = {id}");
        }

        public override PatientData GetById(int id) {
            var reader = m_dbConnector.ExecuteReader($"SELECT * FROM Patients WHERE Id = {id}");
            if (!reader.Read()) {
                return null;
            }

            return new PatientData {
                Name = reader.GetString(1),
                Surname = reader.GetString(2),
                WeakBodySide = (BodySide)reader.GetInt32(3),
                SessionsHistory = reader.GetString(4)
            };
        }

        public override List<PatientData> GetAll() {
            var reader = m_dbConnector.ExecuteReader("SELECT * FROM Patients");
            var patients = new List<PatientData>();
            while (reader.Read()) {
                patients.Add(new PatientData {
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    WeakBodySide = (BodySide)reader.GetInt32(3),
                    SessionsHistory = reader.GetString(4)
                });
            }

            return patients;
        }
    }
}