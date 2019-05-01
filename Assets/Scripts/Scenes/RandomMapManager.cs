using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
    public class RandomMapManager : Photon.PunBehaviour {

        public RandomMap.GameManager maze;
        public int seed = -1;
        public bool isGen = false;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
				stream.SendNext(seed);
			}
			else {
                seed = (int) stream.ReceiveNext();
			}
		}

        void Start() {
            if (PhotonNetwork.isMasterClient) {
                seed = System.DateTime.Now.Second;
            }
        }

        void Update() {
            if (!isGen) {
                if (seed != -1) {
                    Random.InitState(seed);
                    maze.BeginGame();
                }
            }
        }
    }
}
