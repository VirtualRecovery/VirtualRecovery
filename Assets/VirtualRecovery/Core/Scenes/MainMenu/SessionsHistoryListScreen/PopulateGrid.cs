// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 26/04/2025
//  */

using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Managers;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.SessionsHistoryListScreen {
    internal class PopulateGrid : MonoBehaviour {
        public GameObject prefab;
        private PatientsRepository m_patientsRepository;
        private bool m_wasCanvasEnabled;
        
        private void Update() {
            var canvas = transform.parent.transform.parent.transform.parent.GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                m_patientsRepository = new PatientsRepository();
                var patient = GameManager.Instance.GetPatient();
                var sessions = m_patientsRepository.GetSessionsForPatient(patient.Id);
                ClearSessions();
                Populate(sessions);
            }
            m_wasCanvasEnabled = canvas.enabled;
        }
        
        private void ClearSessions() {
            foreach (Transform child in transform) {
                if (child.name != "PlaceholderSessionRecord") {
                    Destroy(child.gameObject);
                }
            }
        }

        private void Populate(List<Session> sessions) {
            foreach (var session in sessions) {
                GameObject newGameObject = (GameObject)Instantiate(prefab, transform);
                FillSessionData(newGameObject, session);
            }
        }

        private void FillSessionData(GameObject newGameObject, Session session) {
            var activity = newGameObject.GetNamedChild("Activity").GetNamedChild("Text");
            var date = newGameObject.GetNamedChild("Date").GetNamedChild("Text");
            var time = newGameObject.GetNamedChild("Time").GetNamedChild("Text");
            var bodySide = newGameObject.GetNamedChild("BodySide").GetNamedChild("Text");
            var difficultyLevel = newGameObject.GetNamedChild("DifficultyLevel").GetNamedChild("Text");
            
            if (activity != null) {
                var roomRepository = new RoomRepository();
                var activityObj = roomRepository.GetActivityById(session.ActivityId);
                activity.GetComponent<TextMeshProUGUI>().text = activityObj.Name;
            }
            
            if (date != null) {
                date.GetComponent<TextMeshProUGUI>().text = session.Date.ToString("dd-MM-yyyy");
            }
            
            if (time != null) {
                time.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(session.Time).ToString(@"hh\:mm\:ss");
            }

            if (bodySide != null) {
                bodySide.GetComponent<TextMeshProUGUI>().text = session.BodySide.ToString();
            }
            
            if (difficultyLevel != null) {
                difficultyLevel.GetComponent<TextMeshProUGUI>().text = session.DifficultyLevel.ToString();
            }
        }
    }
}