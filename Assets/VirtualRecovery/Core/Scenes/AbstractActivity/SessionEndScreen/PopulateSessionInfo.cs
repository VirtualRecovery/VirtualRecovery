// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 13/08/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scenes.AbstractActivity.SessionEndScreen {
    public class PopulateSessionInfo : MonoBehaviour {
        private bool m_wasCanvasEnabled;

        private void Update() {
            var canvas = GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                Populate();
                HandleNextActivityButton();
            }
            m_wasCanvasEnabled = canvas.enabled;
        }

        private void Populate() {
            var patientId = gameObject.GetNamedChild("ExerciseInfo")
                .GetNamedChild("PatientID")
                .GetNamedChild("DataDisplay")
                .GetNamedChild("Text (TMP)");
            var activity = gameObject.GetNamedChild("ExerciseInfo")
                .GetNamedChild("ActivityName")
                .GetNamedChild("DataDisplay")
                .GetNamedChild("Text (TMP)");
            var difficultyLevel = gameObject.GetNamedChild("ExerciseInfo")
                .GetNamedChild("DifficultyLevel")
                .GetNamedChild("DataDisplay")
                .GetNamedChild("Text (TMP)");
            var bodySide = gameObject.GetNamedChild("ExerciseInfo")
                .GetNamedChild("BodySide")
                .GetNamedChild("DataDisplay")
                .GetNamedChild("Text (TMP)");
            var time = gameObject.GetNamedChild("ExerciseInfo")
                .GetNamedChild("Time")
                .GetNamedChild("DataDisplay")
                .GetNamedChild("Text (TMP)");

            if (patientId != null) {
                patientId.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetPatient().Id.ToString();
            }

            if (activity != null) {
                activity.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetCurrentActivity().Name;
            }
            
            if (difficultyLevel != null) {
                difficultyLevel.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetCurrentDifficultyLevel().ToString();
            }
            
            if (bodySide != null) {
                bodySide.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetCurrentBodySide().ToString();
            }
            
            if (time != null) {
                var timeInSeconds = Mathf.FloorToInt(GameManager.Instance.GetSessionDurationTime());
                var minutes = timeInSeconds / 60;
                var seconds = timeInSeconds % 60;
                time.GetComponent<TextMeshProUGUI>().text = $"{minutes:D2}:{seconds:D2}";
            }
        }

        private void HandleNextActivityButton() {
            var returnToMainMenuButton = gameObject.GetNamedChild("ReturnToMenuButton");
            var nextActivityButton = gameObject.GetNamedChild("NextActivityButton");
            
            if (!GameManager.Instance.HasNextActivity()) {
                nextActivityButton.SetActive(false);
                returnToMainMenuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, 0);
            }
            else {
                nextActivityButton.SetActive(true);
                returnToMainMenuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0);
            }
        }
    }
}