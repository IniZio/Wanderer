﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Constant;

namespace Fyp.Game.Network{
    public class PhotonConnection : Photon.PunBehaviour {

        private string gameVersion;

        public void Start() {
            Debug.Log("Start");
            gameVersion = GameConstant.GAME_VERSION;
        }

        public void CreateRoom() {
            Debug.Log("create room");
            if(PhotonNetwork.CreateRoom("ourGame", new RoomOptions() { MaxPlayers = 2 }, null)) {
                Debug.Log("create room success");
            }
            else {
                Debug.Log("create room fail");
            }

        }

        public void JoinRoom() {
            if(PhotonNetwork.JoinRoom("ourGame")) {
                Debug.Log("join room success");
            } else {
                Debug.Log("join room fail");
            }

        }

        public override void OnConnectedToMaster() {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("hii~~");
        }

        public override void OnJoinedLobby() {
            Debug.Log("Joined Lobby");
        }

        public void connect() {
            Debug.Log("connect");
            PhotonNetwork.ConnectUsingSettings(gameVersion);
        }
    }
}