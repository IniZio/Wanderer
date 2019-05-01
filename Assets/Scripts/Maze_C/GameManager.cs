using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.RandomMap {
    public class GameManager : MonoBehaviour {
        // Start is called before the first frame update
        void Start () {
            if (PhotonNetwork.isMasterClient) {
                BeginGame ();
            }
        }

        // Update is called once per frame
        // void Update () {
        //     if (Input.GetKeyDown (KeyCode.Space)) {
        //         RestartGame ();
        //     }
        // }

        public Maze mazePrefab;
        public GameObject mazeIns;

        private Maze mazeInstance;

        private void BeginGame () {
            mazeIns = PhotonNetwork.Instantiate ("mazePrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
            mazeInstance = mazeIns.GetComponent("Maze") as Maze;
            StartCoroutine (mazeInstance.Generate ());
        }

        private void RestartGame () {
            StopAllCoroutines ();
            Destroy (mazeInstance.gameObject);
            BeginGame ();
        }
    }
}