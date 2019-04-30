using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fyp.Game.UI {
    public class ShowRoomList : MonoBehaviour {
        public RoomInfo[] rmList;

        public Text tag;
        // Start is called before the first frame update
        void Start() {
            this.getRoomList();
        }

        // Update is called once per frame
        void Update() {
            this.getRoomList();
            if (this.rmList.Length > 0) {
                this.tag.text = this.rmList[0].Name + " " + this.rmList[0].PlayerCount.ToString() + " / 2";
            }
            else {
                this.tag.text = "There are no room on the server.";
            }
        }

        void getRoomList() {
            rmList = PhotonNetwork.GetRoomList();
        }
    }
}
