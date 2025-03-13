// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private SessionManager m_sessionManager;

        private float m_sessionStartTime;
        private float m_sessionEndTime;

        private Patient m_patient; // TODO: for now these values should be fixed as we're not implementing selection yet
        private Room m_room;
        private Activity m_activity;
        private DifficultyLevel m_difficultyLevel;
        private BodySide m_bodySide;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            m_sessionManager = SessionManager.Instance;
        }

        public void BeginSession() {
            SceneManager.LoadScene(m_room.Name, LoadSceneMode.Single);
            
            // TODO: maybe use some different mechanism
            SceneManager.sceneLoaded += SetSessionStartTime;
            
            m_sessionEndTime = Time.time;
            
            BaseCanvasChanger baseCanvasChanger = FindFirstObjectByType<BaseCanvasChanger>();
            // TODO: change to the session report canvas
            // baseCanvasChanger.ChangeCanvas();
        }

        public float GetSessionDurationTime() {
            if (m_sessionStartTime > 0f) {
                return m_sessionEndTime - m_sessionStartTime;
            }
            return 0f;
        }

        public void CompleteStage(string spaceId, string itemId) {
            // TODO: check if spaceId and itemId match, then show the next Hint, and change progress
        }

        public void SetSessionStartTime(Scene scene, LoadSceneMode mode) => m_sessionStartTime = Time.time;
        
        public void SetPatient(Patient patient) => m_patient = patient;

        public void SetRoom(Room room) => m_room = room;

        public void SetActivity(Activity activity) => m_activity = activity;

        public void SetDifficulty(DifficultyLevel difficultyLevel) => m_difficultyLevel = difficultyLevel;

        public void SetBodyside(BodySide bodySide) => m_bodySide = bodySide;

        private void Start() {

        }
    }
}