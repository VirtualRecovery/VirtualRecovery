// /*
//  * Copyright © 2024 Virtual Recovery
//  * Author: Piotr Lachowicz
//  * Created on: 30/12/2024
//  */

using UnityEngine;

namespace VirtualRecovery {
    internal class ScreenManager : MonoBehaviour {
        internal static ScreenManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start() {

        }
        
        void Update() {

        }
    }
}
