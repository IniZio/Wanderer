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
        GameObject followCamera;
        GameObject mainCamera;
        GameObject p1SpawnPoint;
        GameObject p2SpawnPoint;
        PanelManager menuManager, roomListManager;
        GameObject p1SpawnEffect, p2SpawnEffect;
        Door door;
        ControlScript player1, player2;

        public void Awake () {
            PhotonNetwork.automaticallySyncScene = true;
        }

        public void Start () {
            this.followCamera = GameObject.FindWithTag("FollowCamera");
            this.mainCamera = GameObject.FindWithTag("MainCamera");
            this.followCamera.SetActive(false);
            this.p1SpawnPoint = GameObject.FindWithTag("Player1SpawnPoint");
            this.p2SpawnPoint = GameObject.FindWithTag("Player2SpawnPoint");
            this.menuManager = GameObject.FindWithTag("MenuManager").GetComponent("PanelManager") as PanelManager;
            this.roomListManager = GameObject.FindWithTag("RoomListManager").GetComponent("PanelManager") as PanelManager;
            this.door = GameObject.FindWithTag("WaitingRmDoor").GetComponent("Door") as Door;
        }

        // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		// 	if (stream.isWriting) {
        //         if (PhotonNetwork.isMasterClient) {
        //             stream.SendNext(player1Ready);
        //         }
        //         else {
        //             stream.SendNext(player2Ready);
        //         }
		// 	}
		// 	else {
        //         if (PhotonNetwork.isMasterClient) {
		// 		    player2Ready = (bool)stream.ReceiveNext();
        //         }
        //         else {
		// 		    player1Ready = (bool)stream.ReceiveNext();
        //         }
		// 	}
		// }

        // void Update() {
        //     if (PhotonNetwork.isMasterClient) {
        //         player1Ready = player1.isReady;
        //     }
        //     else {
        //         player2Ready = player2.isReady;
        //     }
        //     if (PhotonNetwork.isMasterClient) {
        //         if (player1Ready && player2Ready) {
        //             if (!this.door.isOpen) {
        //                 OpenDoor();
        //             }
        //         }
        //         else {
        //             if (this.door.isOpen) {
        //                 CloseDoor();
        //             }
        //         }
        //     }
        // }

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
        public void MenuJoinRoom() {
            RoomInfo[] ri = PhotonNetwork.GetRoomList();
            if (ri.Length == 0 || ri[0].PlayerCount == ri[0].MaxPlayers) {
                return;
            }
            if(PhotonNetwork.JoinRoom(PhotonNetwork.GetRoomList()[0].Name)) {
                Debug.Log("join room success");
                roomListManager.CloseCurrent();
            }
            else {
                Debug.Log("join room fail");
                menuManager.OpenPanel(menuManager.initiallyOpen);
            }
        }

        public override void OnConnectedToMaster() {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("OnConnectedToMaster");
        }

        public void ConnectToServer() {
            Debug.Log("connect");
            if(PhotonNetwork.ConnectUsingSettings(GameConstant.GAME_VERSION)) {
                Debug.Log("Connected to server");
                // PhotonNetwork.JoinLobby(TypedLobby.Default);
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
        void setPlayerControl(GameObject player, bool isMaster) {
            ControlScript script = player.GetComponent("ControlScript") as ControlScript;
            // PlayerStatus temp = new PlayerStatus(isMaster);
            // script.setPlayerStatus(temp);
            script.SetIsMe();
            if (isMaster) {
                this.player1 = script;
            }
            else {
                this.player2 = script;
            }
        }

        void  SetObjectLayer(GameObject go, int num) {
            go.layer = num;
            foreach(Transform child in go.transform) {
                SetObjectLayer(child.gameObject, num);
            }
        }

        public override void OnJoinedRoom() {
            this.followCamera.SetActive(true);
            // this.mainCamera.SetActive(false);
            GameObject player;
            SpawnPoint point;
            if(PhotonNetwork.isMasterClient && photonView.isMine) {
                Debug.Log("player1");
                player = PhotonNetwork.Instantiate("PlayerCharacter", this.p1SpawnPoint.transform.position, this.p1SpawnPoint.transform.rotation, 0);
                point = this.p1SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p1SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p1SpawnPoint.transform.position + new Vector3(1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p1SpawnEffect));
                SetObjectLayer(player, 9);
            }
            else {
                Debug.Log("player2");
                player = PhotonNetwork.Instantiate("PlayerCharacter3", this.p2SpawnPoint.transform.position, this.p2SpawnPoint.transform.rotation, 0);
                point = this.p2SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p2SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p2SpawnPoint.transform.position + new Vector3(-1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p2SpawnEffect));
                SetObjectLayer(player, 9);
            }
            setPlayerControl(player, PhotonNetwork.isMasterClient);
            PlayerCamera cameraScirpt = followCamera.GetComponent("PlayerCamera") as PlayerCamera;
            point.onSpawnPlayer();
            cameraScirpt.setCamera(player);
            menuManager.CloseCurrent();
        }

        public override void OnPhotonInstantiate(PhotonMessageInfo info) {
            Debug.Log("hihi111");
            Debug.Log(info.GetType());
        }

        public void DisconnectFromServer() {
            Debug.Log("Disconnect");
            PhotonNetwork.Disconnect();
            UI.ChangeSence.WaitingRoomToMenu();
        }
        public override void OnPhotonPlayerConnected(PhotonPlayer other) {
            if (PhotonNetwork.isMasterClient) {
            }
        }

    }
}
