using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fyp.Game.PlayerCharacter {
    [Serializable]
    public class PlayerMainObj {
        private static PlayerMainObj singleton;
        public static PlayerMainObj Singleton {
            get {
                    if (singleton == null) {
                        singleton = new PlayerMainObj();
                    }
                    return singleton;
            }
        }

        private string masterPlayerName;
        private string clientPlayerName;

        public GameObject masterPlayerChac;
        public GameObject clientPlayerChac;
        private bool enable = false;
        // private PlayerBag bag = new PlayerBag();

        public void setName(string name, bool isMaster) {
            if (isMaster) {
                this.masterPlayerName = name;
            }
            else {
                this.clientPlayerName = name;
            }
        }

        public void setChac(GameObject chac, bool isMaster) {
            if (isMaster) {
                this.masterPlayerChac = chac;
            }
            else {
                this.clientPlayerChac = chac;
            }
        }
    }
}