using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.ResourcesGenerator {
    public class Resources : Photon.PunBehaviour {
        public int hp;
        public int maxHp;
        public bool isGen = false;
        public bool collected = false;
        public string type;
        public GameObject obj;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
				stream.SendNext(hp);
				stream.SendNext(isGen);
				stream.SendNext(collected);
				stream.SendNext(type);
			}
			else {
				hp = (int) stream.ReceiveNext();
				isGen = (bool) stream.ReceiveNext();
				collected = (bool) stream.ReceiveNext();
				type = (string) stream.ReceiveNext();
            }
		}

        public Resources(string type) {
            this.type = type;
            if (type == "Rree") {

            }
            else if (type == "Rock") {

            }
            else if (type == "Metal") {

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
        }

        public GameObject getObj() {
            return this.obj;
        }
    }
}