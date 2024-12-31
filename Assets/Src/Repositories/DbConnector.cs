// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System.Data;
using Mono.Data.Sqlite;

namespace VirtualRecovery {
    internal class DbConnector {
        private string m_dbName = "Data Source=../../Database/VirtualRecovery.db";
        private SqliteConnection m_connection;

        private void CreateConnection() => m_connection = new SqliteConnection(m_dbName);

        public void OpenConnection() {
            if (m_connection == null) {
                CreateConnection();
            }

            m_connection?.Open();
        }

        public void CloseConnection() {
            if (m_connection?.State == ConnectionState.Open) {
                m_connection.Close();
            }
        }

        public SqliteDataReader ExecuteReader(string query) {
            using var command = new SqliteCommand(query, m_connection);
            return command.ExecuteReader();
        }

        public int ExecuteNonQuery(string query) {
            using var command = new SqliteCommand(query, m_connection);
            return command.ExecuteNonQuery();
        }

        public bool TableExists(string tableName) {
            var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
            using var command = new SqliteCommand(query, m_connection);
            using var reader = command.ExecuteReader();
            return reader.Read();
        }
    }
}