// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 26/04/2025
//  */

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
            var beginDate = newGameObject.GetNamedChild("BeginDate").GetNamedChild("Text");
            var endDate = newGameObject.GetNamedChild("EndDate").GetNamedChild("Text");
            var bodySide = newGameObject.GetNamedChild("BodySide").GetNamedChild("Text");
            var difficultyLevel = newGameObject.GetNamedChild("DifficultyLevel").GetNamedChild("Text");
            
            if (activity != null) {
                //activity.GetComponent<TextMeshProUGUI>().text = patient.Id.ToString();
            }
            
            if (beginDate != null) {
                beginDate.GetComponent<TextMeshProUGUI>().text = session.StartDate.ToString(System.Globalization.CultureInfo.CurrentCulture);
            }
            
            if (endDate != null) {
                endDate.GetComponent<TextMeshProUGUI>().text = session.EndDate.ToString(System.Globalization.CultureInfo.CurrentCulture);
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