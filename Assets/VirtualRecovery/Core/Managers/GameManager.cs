// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualRecovery.Core.Managers.Activities;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Fridge;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf;
using VirtualRecovery.Core.Scenes.AbstractActivity;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private float m_sessionStartTime;
        private float m_sessionEndTime;
        
        private bool m_wasRoomAddedd = false;
        private bool m_wasActivityAdded = false;
        private bool m_wasDifficultyLevelAdded = false;
        private bool m_wasBodySideAdded = false;
        
        private bool m_activityEnded = false; // Sometimes EndSession is triggered twice, this flag prevents it from happening

        private Patient m_patient;
        private List<Room> m_rooms = new List<Room>();
        private List<Activity> m_activities = new List<Activity>();
        private List<DifficultyLevel> m_difficultyLevels = new List<DifficultyLevel>();
        private List<BodySide> m_bodySides = new List<BodySide>();
        
        private Room m_currentRoom;
        private Activity m_currentActivity;
        private DifficultyLevel m_currentDifficultyLevel;
        private BodySide m_currentBodySide;
        
        private readonly Dictionary<int, Func<BaseActivity>> m_activityClasses = new() {
            { 4, () => new FridgeActivity()},
            { 1, () => new ShelfActivity()}
        };

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void BeginSession() {
            m_activityEnded = false;
            
            var room = m_rooms[0];
            m_currentRoom = room;
            m_rooms.RemoveAt(0);
            
            SceneManager.LoadScene(room.SceneName, LoadSceneMode.Single);
            SceneManager.sceneLoaded += SetUpSession;
        }
        
        public void SetUpSession(Scene scene, LoadSceneMode mode) {
            var activity = m_activities[0];
            m_currentActivity = activity;
            m_activities.RemoveAt(0);
            
            var difficultyLevel = m_difficultyLevels[0];
            m_currentDifficultyLevel = difficultyLevel;
            m_difficultyLevels.RemoveAt(0);
            
            var bodySide = m_bodySides[0];
            m_currentBodySide = bodySide;
            m_bodySides.RemoveAt(0);
            
            var activityClass = m_activityClasses[activity.Id]();
            activityClass.Load(difficultyLevel, bodySide);
            m_sessionStartTime = Time.time; 
        }

        public void EndSession() {
            if (m_activityEnded) return;
            
            m_activityEnded = true;
            m_sessionEndTime = Time.time;
            Debug.Log($"SIEMANO czas to taki jest o: ");
            ActivityCanvasChanger canvasChanger = FindFirstObjectByType<ActivityCanvasChanger>();
            canvasChanger.ChangeCanvas(new ActivityEventTypeWrapper(ActivityEventType.SessionEnded));
        }

        public float GetSessionDurationTime() {
            if (m_sessionStartTime > 0f) {
                return m_sessionEndTime - m_sessionStartTime;
            }
            return 0f;
        }
        
        public void SetPatient(Patient patient) => m_patient = patient;
        
        public Patient GetPatient() => m_patient;

        public Activity GetLatestActivity() {
            return m_wasActivityAdded ? m_activities.Last() : null;
        }

        public void AddRoom(Room room) {
            if (m_wasRoomAddedd) {
                Debug.LogWarning("Room has already been added. Replacing");
                m_rooms.RemoveAt(m_rooms.Count - 1);
            }
            m_rooms.Add(room);
            m_wasRoomAddedd = true;
        }

        public void AddActivity(Activity activity) {
            if (m_wasActivityAdded) {
                Debug.LogWarning("Activity has already been added. Replacing");
                m_activities.RemoveAt(m_activities.Count - 1);
            }
            m_activities.Add(activity);
            m_wasActivityAdded = true;
        }

        public void AddDifficulty(DifficultyLevel difficultyLevel) {
            if (m_wasDifficultyLevelAdded) {
                Debug.LogWarning("Difficulty level has already been added. Replacing");
                m_difficultyLevels.RemoveAt(m_difficultyLevels.Count - 1);
            }
            m_difficultyLevels.Add(difficultyLevel);
            m_wasDifficultyLevelAdded = true;
        }

        public void AddBodyside(BodySide bodySide) {
            if (m_wasBodySideAdded) {
                Debug.LogWarning("Body side has already been added. Replacing");
                m_bodySides.RemoveAt(m_bodySides.Count - 1);
            }
            m_bodySides.Add(bodySide);
            m_wasBodySideAdded = true;
        }
        
        public void ClearSelectionFlags() {
            m_wasRoomAddedd = false;
            m_wasActivityAdded = false;
            m_wasDifficultyLevelAdded = false;
            m_wasBodySideAdded = false;
        }
        
        public Room GetCurrentRoom() {
            return m_currentRoom;
        }
        
        public Activity GetCurrentActivity() {
            return m_currentActivity;
        }
        
        public DifficultyLevel GetCurrentDifficultyLevel() {
            return m_currentDifficultyLevel;
        }
        
        public BodySide GetCurrentBodySide() {
            return m_currentBodySide;
        }
        
        public bool HasNextActivity() {
            return m_activities.Count > 0;
        }

        public void BackToMainMenu() {
            ClearSelectionFlags();
            m_rooms.Clear();
            m_activities.Clear();
            m_difficultyLevels.Clear();
            m_bodySides.Clear();
            m_patient = null;
            m_currentRoom = null;
            m_currentActivity = null;
            
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            m_activityEnded = false;
        }

        private void Start() {

        }
    }
}