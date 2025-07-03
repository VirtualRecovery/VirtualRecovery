// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualRecovery.Core.Scenes.BaseClasses;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        //private MainMenuModule m_mainMenuManager;
        private SessionManager m_sessionManager;

        private float m_sessionStartTime;
        private float m_sessionEndTime;
        private float m_pauseStartTime;

        private Patient m_patient;
        private Room m_room;
        private Activity m_activity;
        private DifficultyLevel m_difficultyLevel;
        private BodySide m_bodySide;
        
        private bool m_isPaused = false;

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
            SceneManager.LoadScene(m_room.SceneName, LoadSceneMode.Single);
            
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

        public bool IsGamePaused() {
            return m_isPaused;
        }
        
        public void PauseGame() {
            m_pauseStartTime = Time.time;
            m_isPaused = true;
        }

        public void ResumeGame() {
            m_sessionStartTime += m_pauseStartTime;
            m_isPaused = false;
        }

        public void SetSessionStartTime(Scene scene, LoadSceneMode mode) => m_sessionStartTime = Time.time;
        
        public void SetPatient(Patient patient) => m_patient = patient;
        
        public Patient GetPatient() => m_patient;
        
        public Activity GetActivity() => m_activity;

        public void SetRoom(Room room) => m_room = room;

        public void SetActivity(Activity activity) => m_activity = activity;

        public void SetDifficulty(DifficultyLevel difficultyLevel) => m_difficultyLevel = difficultyLevel;

        public void SetBodyside(BodySide bodySide) => m_bodySide = bodySide;

        private void Start() {

        }
    }
}