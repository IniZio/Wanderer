using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.RandomMap {
    public class GameManager : Photon.PunBehaviour {
        public bool isGen = false;

        public int seed = -1;
        public bool isSyncSeed = false;

        void Start() {
            if (PhotonNetwork.isMasterClient) {
                seed = System.DateTime.Now.Second;
            }
        }
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            Debug.Log("-------------------asdf");
			if (stream.isWriting) {
                if (seed == -1) return;
				stream.SendNext(seed);
                isSyncSeed = true;
			}
			else {
                int temp = (int) stream.ReceiveNext();
                if (temp == -1) return;
                seed = temp;
                isSyncSeed = true;
			}
		}

        public Maze mazePrefab;
        public GameObject mazeIns;
        public Object[] temp;
        private Maze mazeInstance;

        public void BeginGame () {
            // mazeIns = PhotonNetwork.InstantiateSceneObject ("mazePrefab", new Vector3(0, 0, 0), Quaternion.identity, 0, temp);
            if (seed != -1) {
                isGen = true;
                Random.InitState(seed);
                mazeInstance = Instantiate(mazePrefab) as Maze;
                StartCoroutine (mazeInstance.Generate ());
            }
        }

        private void RestartGame () {
            StopAllCoroutines ();
            Destroy (mazeInstance.gameObject);
            BeginGame ();
        }
    }
}