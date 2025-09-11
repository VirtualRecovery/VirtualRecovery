// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Wiktoria Kubacka
//  * Created on: 02/09/2025
//  */

using TMPro;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scenes {
    public class UpdateTherapistView : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI patientID;
        [SerializeField] private TextMeshProUGUI activityName;
        [SerializeField] private TextMeshProUGUI difficultyLevel;
        [SerializeField] private TextMeshProUGUI bodySide;
        [SerializeField] private TextMeshProUGUI time;
        public void Start() {
            if (GameManager.Instance == null) {
                Debug.Log("No Game Manager!");
                return;
            }
            StartCoroutine(UpdateExcerciseInfo());
        }

        private System.Collections.IEnumerator UpdateExcerciseInfo() {
            while (true) {
                int secondsFromStart = GameManager.Instance.GetCurrentActivityDurationTime();
                time.text = $"{Mathf.FloorToInt(secondsFromStart / 60f):00}:{secondsFromStart % 60f:00}";
                patientID.text = (GameManager.Instance.GetPatient().Id).ToString();
                activityName.text = GameManager.Instance.GetCurrentActivity().Name;
                difficultyLevel.text = GameManager.Instance.GetCurrentDifficultyLevel().ToString();
                bodySide.text = GameManager.Instance.GetCurrentBodySide().ToString();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}