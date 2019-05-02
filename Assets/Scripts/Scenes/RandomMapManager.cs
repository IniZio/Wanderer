using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;

namespace Fyp.Game.UI {
    public class RandomMapManager : MonoBehaviour {
        public RandomMap.GameManager maze;
        public GameObject Player1, Player2;
        public ControlScript P1Script, P2Script;

        void Awake() {
            this.MapPlayer1();
            this.MapPlayer2();
        }
        void Start() {
        }

        void Update() {
            if (!this.Player1) {
                this.MapPlayer1();
            }
            if (!this.Player2) {
                this.MapPlayer2();
            }
            if (!maze.isGen) {
                Random.InitState(P1Script.randomSeed);
                maze.BeginGame();
            }
        }
        void MapPlayer1() {
            GameObject Player1 = GameObject.FindWithTag("Player1Character");
            if (Player1 != null) {
                this.Player1 = Player1;
                ControlScript script = Player1.GetComponent("ControlScript") as ControlScript;
                this.P1Script = script;
            }
        }
        void MapPlayer2() {
            GameObject Player2 = GameObject.FindWithTag("Player2Character");
            if (Player2 != null) {
                this.Player2 = Player2;
                ControlScript script = Player2.GetComponent("ControlScript") as ControlScript;
                this.P2Script = script;
            }
        }
    }
}
