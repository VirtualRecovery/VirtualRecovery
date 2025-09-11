// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 13/08/2025
//  */

using System.IO;
using UnityEngine;
using VirtualRecovery.DataAccess;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core
{
    [System.Serializable] internal class ConfigurationData {
        public DatabaseConfig database;
        public System.Collections.Generic.List<Room> rooms;
    }

    [System.Serializable] internal class DatabaseConfig {
        public string connectionString;
        public string roomsTableName;
        public string activitiesTableName;
        public string patientTableName;
        public string sessionsTableName;
    }

    // Make sure this runs early
    [DefaultExecutionOrder(-10000)]
    internal class Configuration : MonoBehaviour
    {
        public static Configuration Instance { get; private set; }

        private const string ConfigFileName = "config.json";
        public ConfigurationData configData;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitCoroutine());
        }

        private System.Collections.IEnumerator InitCoroutine()
        {
            yield return LoadConfig();

            var dbPath = EnsureDatabaseFile();
            OverrideConnectionStringDataSource(dbPath);

            var dbConnector = new DbConnector();
            var dbSchemaValidator = new DbSchemaValidator(dbConnector);
            dbSchemaValidator.EnsureTables();

            SeedInitialData();

            Debug.Log($"[Configuration] Init OK. DB: {dbPath}");
        }

        private System.Collections.IEnumerator LoadConfig()
        {
            string path;
#if UNITY_ANDROID
            // On Android StreamingAssets is inside APK → must use UnityWebRequest
            path = Path.Combine(Application.streamingAssetsPath, ConfigFileName);
            using var req = UnityWebRequest.Get(path);
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[Configuration] Failed to load config: {req.error} path={path}");
                yield break;
            }
            var json = req.downloadHandler.text;
#else
            path = Path.Combine(Application.streamingAssetsPath, ConfigFileName);
            if (!File.Exists(path))
            {
                Debug.LogError($"[Configuration] Config not found: {path}");
                yield break;
            }
            var json = File.ReadAllText(path);
#endif
            configData = JsonUtility.FromJson<ConfigurationData>(json);
            if (configData == null) Debug.LogError("[Configuration] Parsed config is null");
        }

        private string EnsureDatabaseFile() {
            var folder = Application.persistentDataPath;
            Directory.CreateDirectory(folder);
            var dbPath = Path.Combine(folder, "VirtualRecovery.db");

            if (!File.Exists(dbPath)) {
                Debug.Log($"[Configuration] Creating new DB at {dbPath}");
            }
            return dbPath;
        }

        private void OverrideConnectionStringDataSource(string dbPath) {
            if (configData == null) return;

            dbPath = dbPath.Replace("\\", "/");

            var cs = configData.database.connectionString;
            if (string.IsNullOrWhiteSpace(cs))
                cs = "Data Source={DB};Version=3;";

            cs = System.Text.RegularExpressions.Regex.Replace(
                cs, @"Data Source\s*=\s*[^;]*", $"Data Source={dbPath}");

            configData.database.connectionString = cs;
            Debug.Log($"[Configuration] Using connectionString: {configData.database.connectionString}");
        }

        private void SeedInitialData() {
            if (configData?.rooms == null || configData.rooms.Count == 0) return;

            var roomRepo = new RoomRepository();
            foreach (var room in configData.rooms) {
                roomRepo.Insert(room);
            }
        }
    }
}
