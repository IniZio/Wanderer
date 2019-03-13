using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void Start () {
            this.followCamera = GameObject.FindWithTag("FollowCamera");
            Debug.Log(this.followCamera);
            this.mainCamera = GameObject.FindWithTag("MainCamera");
            this.followCamera.SetActive(false);
            this.p1SpawnPoint = GameObject.FindWithTag("Player1SpawnPoint");
            this.p2SpawnPoint = GameObject.FindWithTag("Player2SpawnPoint");
            this.menuManager = GameObject.FindWithTag("MenuManager").GetComponent("PanelManager") as PanelManager;
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
            }
        }

        IEnumerator SpawnEffect(GameObject effect) {
            yield return new WaitForSeconds(8);
            PhotonNetwork.Destroy(effect);
        }

        public override void OnJoinedRoom() {
            this.followCamera.SetActive(true);
            GameObject player;
            SpawnPoint point;
            if(PhotonNetwork.isMasterClient && photonView.isMine) {
                Debug.Log("player1");
                player = PhotonNetwork.Instantiate("PlayerCharacter", this.p1SpawnPoint.transform.position, this.p1SpawnPoint.transform.rotation, 0);
                player.SetActive(false);
                point = this.p1SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p1SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p1SpawnPoint.transform.position + new Vector3(1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p1SpawnEffect));
            }
            else {
                Debug.Log("player2");
                player = PhotonNetwork.Instantiate("Player2Character", this.p2SpawnPoint.transform.position, this.p2SpawnPoint.transform.rotation, 0);
                point = this.p2SpawnPoint.GetComponent("SpawnPoint") as SpawnPoint;
                p2SpawnEffect = PhotonNetwork.Instantiate("SpawnEffect", this.p2SpawnPoint.transform.position + new Vector3(-1, 1.6f, 0), this.p1SpawnPoint.transform.rotation, 0);
                StartCoroutine(SpawnEffect(p2SpawnEffect));

            }
            // GameObject camera = PhotonNetwork.Instantiate("FirstCamera", player.transform.position, Quaternion.identity, 0);

            PlayerCamera cameraScirpt = followCamera.GetComponent("PlayerCamera") as PlayerCamera;
            point.onSpawnPlayer();
            cameraScirpt.setCamera(player);
            this.mainCamera.SetActive(false);

            // mainCamera.SetActive(false);
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
