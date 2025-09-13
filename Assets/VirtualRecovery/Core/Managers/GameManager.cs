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
using VirtualRecovery.Core.Managers.Activities.Kitchen.Bread;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Fridge;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Salad;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Soup;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Stove;
using VirtualRecovery.Core.Scenes.AbstractActivity;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private float m_sessionStartTime;
        private float m_sessionEndTime;
        private float m_totalPauseTime = 0f;
        private float m_pauseStartTime = 0f;
        
        private bool m_wasRoomAddedd = false;
        private bool m_wasActivityAdded = false;
        private bool m_wasDifficultyLevelAdded = false;
        private bool m_wasBodySideAdded = false;
        
        private bool m_activityEnded = false; // Sometimes EndSession is triggered twice, this flag prevents it from happening

        private Patient m_patient;
        private Room m_room;
        private Activity m_activity;
        private DifficultyLevel m_difficultyLevel;
        private BodySide m_bodySide;
        
        private bool m_isPaused = false;
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
            { 1, () => new ShelfActivity()},
            { 5, () => new SaladActivity()},
            { 3, () => new BreadActivity()},
            { 2, () => new StoveActivity()},
            { 6, () => new SoupActivity()},
        };

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        public void RestartActivity() {
            if (m_currentRoom == null || m_currentActivity == null || m_currentDifficultyLevel == null || m_currentBodySide == null) {
                Debug.LogError("Cannot restart activity. Current room, activity, difficulty level or body side is null.");
                return;
            }
            ResumeGame();
            SceneManager.LoadScene(m_currentRoom.SceneName, LoadSceneMode.Single);
            SceneManager.sceneLoaded += SetUpRestartedSession;
        }

        public void SetUpRestartedSession(Scene scene, LoadSceneMode mode) {
            SceneManager.sceneLoaded -= SetUpRestartedSession;
            m_totalPauseTime = 0;
            m_totalPauseTime = 0;
            m_sessionStartTime = Time.time; 
            m_activityEnded = false;
            var activityClass = m_activityClasses[m_currentActivity.Id]();
            activityClass.Load(m_currentDifficultyLevel, m_currentBodySide);
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
            SceneManager.sceneLoaded -= SetUpSession;
            if (m_activities.Count == 0)
                return;
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
            m_totalPauseTime = 0;
            m_totalPauseTime = 0;
            m_sessionStartTime = Time.time; 
        }

        public void EndSession() {
            if (m_activityEnded) return;
            m_activityEnded = true;
            
            m_sessionEndTime = Time.time;
            
            Session session = new Session {
                PatientId = m_patient.Id,
                ActivityId = m_currentActivity.Id,
                Date = DateTime.Now,
                Time = GetSessionDurationTime(),
                BodySide = m_currentBodySide,
                DifficultyLevel = m_currentDifficultyLevel
            };
            PatientsRepository patientsRepository = new PatientsRepository();
            patientsRepository.InsertSessionForPatient(m_patient.Id, session);
            
            Debug.Log($"SIEMANO czas to taki jest o: ");
            ActivityCanvasChanger canvasChanger = FindFirstObjectByType<ActivityCanvasChanger>();
            canvasChanger.ChangeCanvas(new ActivityEventTypeWrapper(ActivityEventType.SessionEnded));
        }

        public int GetSessionDurationTime() {
            if (m_sessionStartTime > 0f) {
                return Mathf.RoundToInt(m_sessionEndTime - m_sessionStartTime - m_totalPauseTime);
            }
            return 0;
        }

        public int GetCurrentActivityDurationTime() {
            return Mathf.RoundToInt(Time.time - m_sessionStartTime - m_totalPauseTime);
        }

        public bool IsGamePaused() {
            return m_isPaused;
        }
        
        public void PauseGame() {
            if (!m_isPaused) {
                m_pauseStartTime = Time.time;
                m_isPaused = true;
            }
        }

        public void ResumeGame() {
            if (m_isPaused) {
                m_totalPauseTime += Time.time - m_pauseStartTime;
                m_isPaused = false;
            }
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
            ResumeGame();
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            m_activityEnded = false;
        }
        
        public void ClearData() {
            ClearSelectionFlags();
            m_rooms.Clear();
            m_activities.Clear();
            m_difficultyLevels.Clear();
            m_bodySides.Clear();
            m_patient = null;
            m_currentRoom = null;
            m_currentActivity = null;
            m_activityEnded = false;
        }

        private void Start() {

        }
    }
}