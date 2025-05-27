// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 31/12/2024
//  */

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VirtualRecovery.DataAccess;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core {
    [System.Serializable]
    internal class ConfigurationData {
        public DatabaseConfig database;
        public List<Room> rooms;
    }


    [System.Serializable]
    internal class DatabaseConfig {
        public string connectionString;
        public string roomsTableName;
        public string activitiesTableName;
        public string patientTableName;
        public string sessionsTableName;
    }
    
    internal class Configuration : MonoBehaviour {
        private const string k_configFilePath = "Assets/VirtualRecovery/Core/config.json";
        public static Configuration Instance { get; private set; }

        public ConfigurationData configData;
        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadConfig();
            
            // TODO: Maybe make entry point file? This code should be moved from here
            var dbConnector = new DbConnector();
            var dbSchemaValidator = new DbSchemaValidator(dbConnector);
            dbSchemaValidator.EnsureTables();
            
            var roomRepository = new RoomRepository();
            foreach (var room in configData.rooms) {
                roomRepository.Insert(room);
            }
        }
        
        void Start() {
        }
        
        public void LoadConfig() {
            if (File.Exists(k_configFilePath)) {
                string json = File.ReadAllText(k_configFilePath);
                configData = JsonUtility.FromJson<ConfigurationData>(json);
                
                if (configData != null) {
                    Debug.Log("Config loaded successfully.");
                } else {
                    Debug.LogError("Configuration data is null after loading.");
                }
            } else {
                Debug.LogError($"Config file not found at path: {k_configFilePath}");
            }
        }
    }
}