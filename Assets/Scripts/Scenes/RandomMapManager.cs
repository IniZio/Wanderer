﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;
using Fyp.Game.Carmera;

namespace Fyp.Game.UI {
    public class RandomMapManager : MonoBehaviour {
        public RandomMap.GameManager maze;
        public GameObject Player1, Player2, P1point, P2point, FollowCamera;
        public ControlScript P1Script, P2Script;
        public bool firstMap = false;

        void Awake() {
        }
        void Start() {
            this.MapPlayer1();
            this.MapPlayer2();
        }

        void Update() {
            if (!this.Player1 && maze.isGen) {
                this.MapPlayer1();
            }
            if (!this.Player2 && maze.isGen) {
                this.MapPlayer2();
            }
            if (!maze.isGen) {
                Random.InitState(P1Script.randomSeed);
                maze.BeginGame();
            }
            if (this.Player1 && this.Player2 && this.maze.isGen && !this.firstMap) {
                TransportToSpawnPoint();
            }
        }
        void MapPlayer1() {
            GameObject Player1 = GameObject.FindWithTag("Player1Character");
            if (Player1 != null) {
                this.Player1 = Player1;
                P1point = GameObject.FindWithTag("Player1SpawnPoint");
                ControlScript script = Player1.GetComponent("ControlScript") as ControlScript;
                if(script.getIsMe()) {
                    PlayerCamera cameraScirpt = FollowCamera.GetComponent("PlayerCamera") as PlayerCamera;
                    cameraScirpt.setCamera(this.Player1);
                }
                this.P1Script = script;
            }
        }
        void MapPlayer2() {
            GameObject Player2 = GameObject.FindWithTag("Player2Character");
            if (Player2 != null) {
                this.Player2 = Player2;
                P2point = GameObject.FindWithTag("Player2SpawnPoint");
                ControlScript script = Player2.GetComponent("ControlScript") as ControlScript;
                if(script.getIsMe()) {
                    PlayerCamera cameraScirpt = FollowCamera.GetComponent("PlayerCamera") as PlayerCamera;
                    cameraScirpt.setCamera(this.Player2);
                }
                this.P2Script = script;
            }
        }

        void TransportToSpawnPoint() {
            this.firstMap = true;
            this.Player2.transform.position = P2point.transform.position;
            this.Player1.transform.position = P1point.transform.position;
        }
    }
}