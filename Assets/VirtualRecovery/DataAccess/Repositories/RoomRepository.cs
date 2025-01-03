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
            var query = $"INSERT INTO {m_roomsTableName} (Name, Description)" +
                        "VALUES (@Name, @Description)";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@Name", entity.Name),
                           ("@Description", entity.Description))) {

                    if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                        throw new Exception("No rows were updated.");
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
        }

        public void Update(int id, Room entity) {
            var query = $"UPDATE {m_roomsTableName} SET Name = @Name, Description = @Description WHERE Id = @Id";
            
            m_dbConnector.OpenConnection();
            try {
                using (var command = m_dbConnector.CreateCommand(query,
                           ("@Name", entity.Name),
                           ("@Description", entity.Description),
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
                            Description = activitiesReader.GetString(3),
                            IsBodySideDifferentiated = activitiesReader.GetBoolean(4)
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
                            Description = reader.GetString(2),
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
                            Description = reader.GetString(2),
                            Activities = GetActivitiesForRoom(reader.GetInt32(0))
                        };

                        rooms.Add(room);
                    }
                }
            }
            finally {
                m_dbConnector.CloseConnection();
            }
            return rooms;
        }
    }
}