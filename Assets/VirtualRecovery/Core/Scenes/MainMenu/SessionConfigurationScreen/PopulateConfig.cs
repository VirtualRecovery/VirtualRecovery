// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 27/05/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Managers;

namespace VirtualRecovery.Core.Scenes.MainMenu.SessionConfigurationScreen {
    public class PopulateConfig : MonoBehaviour {
        private bool m_wasCanvasEnabled;

        private static List<TMP_Dropdown.OptionData> m_twoSideOptions = new List<TMP_Dropdown.OptionData> {
            new TMP_Dropdown.OptionData("Lewa"),
            new TMP_Dropdown.OptionData("Prawa")
        };

        private static List<TMP_Dropdown.OptionData> m_oneSideOptions = new List<TMP_Dropdown.OptionData> {
            new TMP_Dropdown.OptionData("Obie")
        };

        private void Update() {
            var canvas = transform.parent.GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                Populate();
            }
            m_wasCanvasEnabled = canvas.enabled;
        }

        private void Populate() {
            var patientId = gameObject.GetNamedChild("PatientID").GetNamedChild("Button").GetNamedChild("Text (TMP)");
            var activity = gameObject.GetNamedChild("Activity").GetNamedChild("Button").GetNamedChild("Text (TMP)");
            var bodySide = gameObject.GetNamedChild("Bodyside").GetNamedChild("Dropdown");

            if (patientId != null) {
                patientId.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetPatient().Id.ToString();
            }

            if (activity != null) {
                activity.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetActivity().Name;
            }

            if (bodySide != null) {
                var tmpDropdown = bodySide.GetComponent<TMP_Dropdown>();
                if (tmpDropdown != null) {
                    if (!GameManager.Instance.GetActivity().IsBodySideDifferentiated) {
                        tmpDropdown.interactable = false;
                        tmpDropdown.options = new List<TMP_Dropdown.OptionData>(m_oneSideOptions);
                    }
                    else {
                        tmpDropdown.interactable = true;
                        tmpDropdown.options = new List<TMP_Dropdown.OptionData>(m_twoSideOptions);
                    }
                }
            }
        }
    }
}