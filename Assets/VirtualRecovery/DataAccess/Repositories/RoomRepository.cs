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
    internal class RoomRepository : IRepository<Room> {
        private readonly DbConnector m_dbConnector;
        private readonly string m_roomsTableName;
        private readonly string m_activitiesTableName;

        public RoomRepository() {
            m_dbConnector = new DbConnector();
            
            var config = Configuration.Instance;
            m_roomsTableName = config.configData.database.roomsTableName;
            m_activitiesTableName = config.configData.database.activitiesTableName;
        } 

        public void Insert(Room entity) {
            var insertQuery = $"INSERT INTO {m_roomsTableName} (Name) VALUES (@Name)";
            var getIdQuery = "SELECT last_insert_rowid()";

            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(insertQuery, ("@Name", entity.Name))) {
                    if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                        throw new Exception("No rows were inserted.");
                    }
                }

                using (var idCommand = m_dbConnector.CreateCommand(getIdQuery))
                using (var reader = idCommand.ExecuteReader()) {
                    if (reader.Read()) {
                        entity.Id = Convert.ToInt32(reader[0]);
                    }
                }
                m_dbConnector.CloseConnection();
                InsertActivitiesForRoom(entity);
            }
            finally {
                try {
                    m_dbConnector.CloseConnection();
                } catch (Exception ex) {}
            }
        }


        public void Update(int id, Room entity) {
            var query = $"UPDATE {m_roomsTableName} SET Name = @Name WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@Name", entity.Name),
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
            var query = $"DELETE FROM {m_roomsTableName} WHERE Id = @Id";
            
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
        
        private List<Activity> GetActivitiesForRoom(int roomId) {
            var activities = new List<Activity>();
            var activitiesQuery = $"SELECT * FROM {m_activitiesTableName} WHERE RoomId = @RoomId";
            
            m_dbConnector.OpenConnection();
            try {
                using (var activitiesCommand = m_dbConnector.CreateCommand(activitiesQuery, ("@RoomId", roomId)))
                using (var activitiesReader = activitiesCommand.ExecuteReader()) {
                    while (activitiesReader.Read()) {
                        var activity = new Activity {
                            Id = activitiesReader.GetInt32(0),
                            RoomId = activitiesReader.GetInt32(1),
                            Name = activitiesReader.GetString(2),
                            IsBodySideDifferentiated = activitiesReader.GetBoolean(3)
                        };
                        activities.Add(activity);
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return activities;
        }

        public Room GetById(int id) {
            var query = $"SELECT * FROM {m_roomsTableName} WHERE Id = @Id";
    
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query, ("@Id", id)))
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read()) {
                        var room = new Room {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Activities = GetActivitiesForRoom(id)
                        };

                        return room;
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return null;
        }

        public List<Room> GetAll() {
            var query = $"SELECT * FROM {m_roomsTableName}";
            var rooms = new List<Room>();
    
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query))
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var room = new Room {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                        };

                        rooms.Add(room);
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            foreach (var room in rooms) {
                room.Activities = GetActivitiesForRoom(room.Id);
            }
            return rooms;
        }
        
        private void InsertActivitiesForRoom(Room room) {
            var query = $"INSERT INTO {m_activitiesTableName} (RoomId, Name, IsBodySideDifferentiated)" +
                        "VALUES (@RoomId, @Name, @IsBodySideDifferentiated)";
        
            m_dbConnector.OpenConnection();
            try {
                foreach (var activity in room.Activities) {
                    using (var command = m_dbConnector.CreateCommand(query,
                               ("@RoomId", room.Id),
                               ("@Name", activity.Name),
                               ("@IsBodySideDifferentiated", activity.IsBodySideDifferentiated))) {

                        if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                            throw new Exception("No rows were updated.");
                        }
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
        }
    }
}