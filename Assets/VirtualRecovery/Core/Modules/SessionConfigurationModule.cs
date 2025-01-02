// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;
using VirtualRecovery.Repositories;

namespace VirtualRecovery.Managers.Modules {
    internal class SessionConfigurationModule : MonoBehaviour {
        private RoomRepository m_roomRepository;

        private void Awake() {
            m_roomRepository = new RoomRepository();
        }

        void Start() { }

        void Update() { }
    }
}