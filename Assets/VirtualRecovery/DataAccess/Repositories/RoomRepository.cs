// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;
using VirtualRecovery.DataModels;

namespace VirtualRecovery.Repositories {
    internal class RoomRepository : IRepository<Room> {
        private readonly DbConnector m_dbConnector;
        private readonly string m_roomsTableName;
        private readonly string m_activitiesTableName;

        public RoomRepository() {
            m_dbConnector = new DbConnector();
            m_dbConnector.OpenConnection();
            
            var config = Configuration.Instance;
            m_roomsTableName = config.configData.database.roomsTableName;
            m_activitiesTableName = config.configData.database.activitiesTableName;
            
            EnsureTables();
        } 

        private string GenerateCreateRoomsTableQuery() {
            return $"CREATE TABLE {m_roomsTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "Name TEXT NOT NULL," +
                   "Description TEXT NOT NULL" +
                   ")";
        }
    
        private string GenerateCreateActivitiesTableQuery() {
            return $"CREATE TABLE {m_activitiesTableName} (" +
                   "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                   "RoomId INTEGER NOT NULL," +
                   "Name TEXT NOT NULL," +
                   "Description TEXT NOT NULL," +
                   "IsBodySideDifferentiated BOOLEAN NOT NULL," +
                   $"FOREIGN KEY(RoomId) REFERENCES {m_roomsTableName}(Id)" +
                   ")";
        }
    
        private void EnsureTables() {
            if (!m_dbConnector.TableExists(m_roomsTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreateRoomsTableQuery());
            }
            
            if (!m_dbConnector.TableExists(m_activitiesTableName)) {
                m_dbConnector.ExecuteQuery(GenerateCreateActivitiesTableQuery());
            }
        }

        public void Insert(Room entity) {
            var query = $"INSERT INTO {m_roomsTableName} (Name, Description)" +
                        "VALUES (@Name, @Description)";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.Name),
                       ("@Description", entity.Description))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Update(int id, Room entity) {
            var query = $"UPDATE {m_roomsTableName} SET Name = @Name, Description = @Description WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query,
                       ("@Name", entity.Name),
                       ("@Description", entity.Description),
                       ("@Id", id))) {
                
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }

        public void Delete(int id) {
            var query = $"DELETE FROM {m_roomsTableName} WHERE Id = @Id";
            
            using (var command = m_dbConnector.CreateCommand(query, ("@Id", id))) {
                if (m_dbConnector.ExecuteNonQuery(command) == 0) {
                    throw new Exception("No rows were updated.");
                }
            }
        }
        
        private List<Activity> GetActivitiesForRoom(int roomId) {
            var activities = new List<Activity>();
            var activitiesQuery = $"SELECT * FROM {m_activitiesTableName} WHERE RoomId = @RoomId";
    
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
    
            return activities;
        }

        public Room GetById(int id) {
            var query = $"SELECT * FROM {m_roomsTableName} WHERE Id = @Id";
    
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

            return null;
        }

        public List<Room> GetAll() {
            var query = $"SELECT * FROM {m_roomsTableName}";
            var rooms = new List<Room>();
    
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

            return rooms;
        }
    }
}