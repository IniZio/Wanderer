using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Constant;
using Fyp.Game.PlayerCharacter;

namespace Fyp.Game.Network {
    public class PhotonRoomConnection : Photon.PunBehaviour {
        public void Start() {
            Debug.Log("Start");
        }

        public void CreateRoom(string playerName) {
            Debug.Log("create room");
            if (PhotonNetwork.CreateRoom(playerName + " s Room", new RoomOptions() { MaxPlayers = 2 }, null)) {
                Debug.Log("create room success");
                NetworkChangeScene.ChangeToScene((int) GameConstant.ScenceName.WaitingRoom);
            }
            else {
                Debug.Log("create room fail");
                // Open a dialog??
            }
        }

        public void JoinRoom(string targetName) {
            if(PhotonNetwork.JoinRoom(targetName + " s Room")) {
                Debug.Log("join room success");
                NetworkChangeScene.ChangeToScene((int) GameConstant.ScenceName.WaitingRoom);

            } else {
                Debug.Log("join room fail");
                // Open a dialog?? 
            }
        }

        public override void OnConnectedToMaster() {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("OnConnectedToMaster");
        }

        public override void OnJoinedLobby() {
            Debug.Log("OnJoinedLobby");
        }

        public void connect() {
            Debug.Log("connect");
            PhotonNetwork.ConnectUsingSettings(GameConstant.GAME_VERSION);
        }
    }
}
