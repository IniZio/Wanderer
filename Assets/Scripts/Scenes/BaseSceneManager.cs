﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;
using Fyp.Game.Carmera;
using Fyp.Game.Network;
using Fyp.Game.ResourcesGenerator;

namespace Fyp.Game.UI {
    public class BaseSceneManager : MonoBehaviour {
        public GameObject P1point, P2point;
        public GameObject Player1, Player2;
        public ControlScript P1Script, P2Script;
        public GameObject FollowCamera;
        public ResourcesGenerator.ResourcesGenerator resGen;

        void Awake() {
            this.MapPlayer1();
            this.MapPlayer2();
        }

        void Start() {
            if (!this.resGen) {
                this.MapResGen();
            }
        }

        void Update() {
            if (!this.resGen.generated) {
                resGen.randomGen();
            }
            if (!this.Player1 || !this.P1Script) {
                this.MapPlayer1();
            }
            if (!this.Player2 || !this.P2Script) {
                this.MapPlayer2();
            }
            if (this.P1Script.getStandingBaseGate() && this.P2Script.getStandingBaseGate()) {
                this.P1Script.exitBaseGate();
                this.P2Script.exitBaseGate();
                NetworkChangeScene.AllPlayerChangeScene("Dungeon");
            }
        }

        void MapPlayer1() {
            GameObject Player1 = GameObject.FindWithTag("Player1Character");
            if (Player1 != null) {
                this.Player1 = Player1;
                this.Player1.transform.position = P1point.transform.position;
                ControlScript script = Player1.GetComponent("ControlScript") as ControlScript;
                if(script.getIsMe()) {
                    PlayerCamera cameraScirpt = FollowCamera.GetComponent("PlayerCamera") as PlayerCamera;
                    cameraScirpt.setCamera(this.Player1);
                }
                this.P1Script = script;
                Player1.GetComponent<ControlScript>().health = 60;

            }
        }
        void MapPlayer2() {
            GameObject Player2 = GameObject.FindWithTag("Player2Character");
            if (Player2 != null) {
                this.Player2 = Player2;
                this.Player2.transform.position = P2point.transform.position;
                ControlScript script = Player2.GetComponent("ControlScript") as ControlScript;
                if(script.getIsMe()) {
                    PlayerCamera cameraScirpt = FollowCamera.GetComponent("PlayerCamera") as PlayerCamera;
                    cameraScirpt.setCamera(this.Player2);
                }
                this.P2Script = script;
                Player2.GetComponent<ControlScript>().health = 60;
            }
        }

        void MapResGen() {
            GameObject obj = GameObject.FindWithTag("ResourceGen");
            if (obj != null) {
                ResourcesGenerator.ResourcesGenerator rg = obj.GetComponent("ResourcesGenerator") as ResourcesGenerator.ResourcesGenerator;
                if (rg != null) {
                    this.resGen = rg;
                }
                else {
                    Debug.LogError("cannot get resGen");
                }
            }
        }
    }
}