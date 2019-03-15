using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Fyp.Constant;
using Fyp.Game.Carmera;
using Fyp.Game.PlayerControl;
using Fyp.Game.UI;

namespace Fyp.Game.Network {
    public class PhotonRoomConnection : Photon.PunBehaviour {
        public bool spawned = false;
        GameObject followCamera;
        GameObject mainCamera;
        GameObject p1SpawnPoint;
        GameObject p2SpawnPoint;
        PanelManager menuManager;
        GameObject p1SpawnEffect, p2SpawnEffect;
        Door door;
        ControlScript player1, player2;
        int test = 0;

        public void Start () {
            this.followCamera = GameObject.FindWithTag("FollowCamera");
            this.mainCamera = GameObject.FindWithTag("MainCamera");
            this.followCamera.SetActive(false);
            this.p1SpawnPoint = GameObject.FindWithTag("Player1SpawnPoint");
            this.p2SpawnPoint = GameObject.FindWithTag("Player2SpawnPoint");
            this.menuManager = GameObject.FindWithTag("MenuManager").GetComponent("PanelManager") as PanelManager;
            this.door = GameObject.FindWithTag("WaitingRmDoor").GetComponent("Door") as Door;
        }

        public void Update() {
            if (this.player2 != null && this.player1 != null) {
                if (player1.readyForPlayer && player2.readyForPlayer) {
                    if (!this.door.isOpen) {
                        OpenDoor();
                    }
                }
                else {
                    if (this.door.isOpen) {
                        CloseDoor();
                    }
                }
            }
        }

        public void CreateRoom(string playerName) {
            Debug.Log("create room");
            if (PhotonNetwork.CreateRoom(playerName, new RoomOptions() { MaxPlayers = 2 }, null)) {
                Debug.Log("create room success");
            }
            else {
                Debug.Log("create room fail");
                menuManager.OpenPanel(menuManager.initiallyOpen);
            }
        }

        public void JoinRoom(string roomName) {
            if(PhotonNetwork.JoinRoom(roomName)) {
                Debug.Log("join room success");
            }
            else {
                Debug.Log("join room fail");
                menuManager.OpenPanel(menuManager.initiallyOpen);
            }
        }

        // public override void OnConnectedToMaster() {
        //     PhotonNetwork.JoinLobby(TypedLobby.Default);
        //     Debug.Log("OnConnectedToMaster");
        // }

        public void ConnectToServer() {
            Debug.Log("connect");
            if(PhotonNetwork.ConnectUsingSettings(GameConstant.GAME_VERSION)) {
                Debug.Log("Connected to server");
                // NetworkChangeScene.ChangeToScene((int) GameConstant.ScenceName.WaitingRoom);
                UI.ChangeSence.MenuToWaitingRoom();
            } else {
                Debug.Log("Connect to server failed");
                UI.ChangeSence.WaitingRoomToMenu();

            }
        }

        IEnumerator SpawnEffect(GameObject effect) {
            yield return new WaitForSeconds(8);
            PhotonNetwork.Destroy(effect);
        }

        [PunRPC]
        void OpenDoor() {
            this.door.OpenDoor();
        }

        [PunRPC]
        void CloseDoor() {
            this.door.CloseDoor();
        }

        [PunRPC]
        void setPlayerControl(GameObject player, bool isMaster) {
            ControlScript script = player.GetComponent("ControlScript") as ControlScript;
            if (isMaster) {
                this.player1 = script;
                this.player1.isMaster = true;
            }
            else {
                this.player2 = script;
                this.player2.isMaster = false;
            }
        }

        public override void OnJoinedRoom() {
            Debug.Log("hihihi: " + this.test.ToString());
            this.followCamera.SetActive(true);
            GameObject player;
            SpawnPoint point;
            if(PhotonNetwork.isMasterClient && photonView.isMine) {
                Debug.Log("player1");
                player = PhotonNetwork.Instantiate("PlayerCharacter", this.p1SpawnPoint.transform.position, this.p1SpawnPoint.transform.rotation, 0);
                point = this.p1SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p1SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p1SpawnPoint.transform.position + new Vector3(1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p1SpawnEffect));
                setPlayerControl(player, true);
            }
            else {
                Debug.Log("player2");
                player = PhotonNetwork.Instantiate("PlayerCharacter2", this.p2SpawnPoint.transform.position, this.p2SpawnPoint.transform.rotation, 0);
                point = this.p2SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p2SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p2SpawnPoint.transform.position + new Vector3(-1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p2SpawnEffect));
                setPlayerControl(player, false);

            }
            PlayerCamera cameraScirpt = followCamera.GetComponent("PlayerCamera") as PlayerCamera;
            point.onSpawnPlayer();
            cameraScirpt.setCamera(player);
            this.mainCamera.SetActive(false);
        }

        // public override void OnPhotonInstantiate(PhotonMessageInfo info) {
        //     info.sender.TagObject = this.GameObject;
        // }

        public void DisconnectFromServer() {
            Debug.Log("Disconnect");
            PhotonNetwork.Disconnect();
            UI.ChangeSence.WaitingRoomToMenu();
        }
    }
}
