using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.ResourcesGenerator {
    public class Resources : MonoBehaviour {
        int hp;
        int maxHp;
        bool isGen = false;
        bool collected = false;
        string type;
        public GameObject obj;

        public Resources(string type) {
            this.type = type;
            if (type == "tree") {

            }
            else if (type == "stone") {

            }
            else if (type == "metal") {

            }
        }

        void Start() {
            this.hp = 3;
            this.maxHp = 3;
            this.isGen  = true;
            this.collected = false;
        }

        public void setType(string type) {
            this.type = type;
        }

        void attacked(GameObject tool) {
            if (tool.GetComponent("tools")) {

            }
        }
        public void setObj(GameObject obj) {
            this.obj = obj;
            this.isGen = true;
        }

        public GameObject getObj() {
            return this.obj;
        }
    }
}