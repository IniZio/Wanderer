using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Fyp.Game.PlayerCharacter {
    public class PlayerMainObj {
        private static PlayerMainObj sInstance = null;
        public static PlayerMainObj Instance {
            get {
                if (sInstance == null) {
                    sInstance = new PlayerMainObj();
                }
                return sInstance;
            }
        }

        private GameObject playerChar;
        private PlayerData playerData;
        private PlayerState playerSate;

        private bool enable = false;
        // private PlayerBag bag = new PlayerBag();

        public void setChar(GameObject character, bool isMaster) {
            this.playerChar = character;
        }
    }
}