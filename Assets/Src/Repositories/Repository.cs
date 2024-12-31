// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 31/12/2024
//  */

using System.Collections.Generic;

namespace VirtualRecovery {
    internal abstract class Repository<T> {
        protected DbConnector m_dbConnector;

        protected Repository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            EnsureTable();
        }

        protected abstract string GetTableName();

        private void EnsureTable() {
            if (!m_dbConnector.TableExists(GetTableName())) {
                m_dbConnector.ExecuteNonQuery(GenerateCreateTableQuery());
            }
        }

        protected abstract string GenerateCreateTableQuery();

        public abstract void Insert(T entity);
        public abstract void Update(int id, T entity);
        public abstract void Delete(int id);
        public abstract T GetById(int id);
        public abstract List<T> GetAll();
    }
}