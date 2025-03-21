﻿// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using Mono.Data.Sqlite;
using VirtualRecovery.Core;

namespace VirtualRecovery.DataAccess {
    internal class DbConnector {
        private SqliteConnection m_connection;
        private readonly string m_dbName;

        public DbConnector() {
            var config = Configuration.Instance;
            m_dbName = config.configData.database.connectionString;
        }

        private SqliteConnection CreateConnection() => new (m_dbName);

        public void OpenConnection() => (m_connection ??= CreateConnection()).Open();

        public void CloseConnection() => m_connection?.Close();

        public SqliteCommand CreateCommand(string query, params (string, object)[] parameters) {
            var command = new SqliteCommand(query, m_connection);
            
            foreach (var (name, value) in parameters) {
                command.Parameters.AddWithValue(name, value);
            }

            return command;
        }
        
        public SqliteDataReader ExecuteReader(string query) => new SqliteCommand(query, m_connection).ExecuteReader();
        
        public int ExecuteNonQuery(SqliteCommand command) => command.ExecuteNonQuery();
        
        public int ExecuteQuery(string query) => new SqliteCommand(query, m_connection).ExecuteNonQuery();
    }
}