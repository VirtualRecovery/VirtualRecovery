// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualRecovery.Core.Managers.Activities;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Fridge;
using VirtualRecovery.Core.Managers.Activities.Kitchen.Shelf;
using VirtualRecovery.DataAccess.DataModels;

namespace VirtualRecovery.Core.Managers {
    internal class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        private float m_sessionStartTime;
        private float m_sessionEndTime;

        private Patient m_patient;
        private Room m_room;
        private Activity m_activity;
        private DifficultyLevel m_difficultyLevel;
        private BodySide m_bodySide;
        
        private readonly Dictionary<int, Func<BaseActivity>> m_activities = new() {
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
            SceneManager.LoadScene(m_room.SceneName, LoadSceneMode.Single);
            SceneManager.sceneLoaded += SetUpSession;
        }
        
        public void SetUpSession(Scene scene, LoadSceneMode mode) {
            var activity = m_activities[m_activity.Id]();
            activity.Load(m_difficultyLevel, m_bodySide);
            m_sessionStartTime = Time.time; 
        }

        public void EndSession() {
            m_sessionEndTime = Time.time;
            Debug.Log($"SIEMANO czas to taki jest o: {GetSessionDurationTime()}");
            
            // TODO: change to the session report canvas
            // BaseCanvasChanger baseCanvasChanger = FindFirstObjectByType<BaseCanvasChanger>();
            // baseCanvasChanger.ChangeCanvas();
        }

        public float GetSessionDurationTime() {
            if (m_sessionStartTime > 0f) {
                return m_sessionEndTime - m_sessionStartTime;
            }
            return 0f;
        }
        
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