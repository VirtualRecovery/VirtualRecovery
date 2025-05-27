// /*
//  * Copyright © 2025 Virtual Recovery
//  * Author: Mateusz Kaszubowski
//  * Created on: 23/04/2025
//  */

using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using VirtualRecovery.Core.Scenes.MainMenu.Common;
using VirtualRecovery.DataAccess.DataModels;
using VirtualRecovery.DataAccess.Repositories;

namespace VirtualRecovery.Core.Scenes.MainMenu.ActivitySelectionScreen {
    internal class PopulateGrid : MonoBehaviour {
        public GameObject prefab;
        private RoomRepository m_roomRepository;
        private bool m_wasCanvasEnabled;
        
        private void Update() {
            var canvas = transform.parent.transform.parent.transform.parent.GetComponent<Canvas>();
            if (canvas.enabled && !m_wasCanvasEnabled) {
                m_roomRepository = new RoomRepository();
                var rooms = m_roomRepository.GetAll();
                ClearActivities();
                Populate(rooms);
            }
            m_wasCanvasEnabled = canvas.enabled;
        }
        
        private void ClearActivities() {
            foreach (Transform child in transform) {
                if (child.name != "ActivityPlaceholderRecord") {
                    Destroy(child.gameObject);
                }
            }
        }

        private void Populate(List<Room> rooms) {
            foreach (var room in rooms) {
                foreach (var activity in room.Activities) {
                    GameObject newGameObject = (GameObject)Instantiate(prefab, transform);
                    FillActivityData(newGameObject, room, activity);
                }
            }
        }

        private void FillActivityData(GameObject newGameObject, Room room, Activity activity) {
            newGameObject.GetComponent<ActivityData>().room = room;
            newGameObject.GetComponent<ActivityData>().activity = activity;
            
            var name = newGameObject.GetNamedChild("Name").GetNamedChild("Text");
            var roomName = newGameObject.GetNamedChild("Room").GetNamedChild("Text");
            var bodySideDifferentiated = newGameObject.GetNamedChild("BodySideDifferentiated").GetNamedChild("Text");
            
            if (name != null) {
                name.GetComponent<TextMeshProUGUI>().text = activity.Name;
            }
            
            if (roomName != null) {
                roomName.GetComponent<TextMeshProUGUI>().text = room.Name;
            }
            
            if (bodySideDifferentiated != null) {
                bodySideDifferentiated.GetComponent<TextMeshProUGUI>().text = activity.IsBodySideDifferentiated ? "Tak" : "Nie";
            }
        }
    }
}