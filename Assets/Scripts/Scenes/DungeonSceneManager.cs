using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;
using Fyp.Game.Carmera;
using Leap.Unity.Interaction;

namespace Fyp.Game.UI {

    public class DungeonSceneManager : Photon.PunBehaviour, IPunObservable
    {
        int misssion3 = 0;
        int HintsCount = 0;
        int abc = -1;

        public GameObject P1point, P2point;
        public GameObject Player1, Player2;
        ControlScript P1Script, P2Script;
        public GameObject FollowCamera;
        public GameObject MainDoor;
        public GameObject Mission1Floor;
        public GameObject Mission3Hints;
        public Light[] Lights = new Light[9];
        public DungeonMission missionManager;


        public bool[] ButtonArray = {false, false, false, false, false, false, false, false, false};
        public bool[] LightArray = {false, false, false, false, false, false, false, false, false};
        public bool[] Mission1Array = {false, false};

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
                abc = 3;
                Debug.Log("Sync button array" + ButtonArray[3]);
				stream.SendNext(ButtonArray);
                stream.SendNext(LightArray);
                stream.SendNext(Mission1Array);
                stream.SendNext(misssion3);
                stream.SendNext(HintsCount);
                photonView.RPC("ForceUpdate", PhotonTargets.All, ButtonArray, LightArray, Mission1Array, misssion3, HintsCount);

            }
			else {
                ButtonArray = (bool[]) stream.ReceiveNext();
                LightArray = (bool[]) stream.ReceiveNext();
                Mission1Array = (bool[]) stream.ReceiveNext();
                misssion3 = (int) stream.ReceiveNext();
                HintsCount = (int) stream.ReceiveNext();
                Debug.Log("Receive button array" + ButtonArray[3]);
                Debug.Log("Receive abc" + abc);
            }
		}

        [PunRPC]
        public void ForceUpdate(bool[] a, bool[] b, bool[] c, int d, int e)
        {
            ButtonArray = a;
            LightArray = b;
            Mission1Array = c;
            misssion3 = d;
            HintsCount = e;
        }

        private void Start()

        {

            missionManager = GetComponent<DungeonMission>();
        }

        
        void Awake() {
            this.MapPlayer1();
            this.MapPlayer2();
            this.MainDoor.SetActive(true);
            this.Mission3Hints.SetActive(false);
            for(int i = 0; i < Lights.Length; i++){
                Lights[i].GetComponent<Light>().enabled = !Lights[i].GetComponent<Light>().enabled;
                //ButtonArray[i] = false;
            }
        }

        
        void Update() {
            int LightCount = 0;
            for(int i = 0; i < Lights.Length; i++){
                    if(ButtonArray[i] == true){
                    Lights[i].GetComponent<Light>().enabled = true;
                    LightCount++;
                 }
            }
            if(HintsCount == 250){
                Mission3Hints.SetActive(true);
            }

           if (Input.GetKeyDown(KeyCode.Alpha1)) {
                //Lights[0].GetComponent<Light>().enabled = !Lights[0].GetComponent<Light>().enabled;
                for (int i = 0; i < Lights.Length; i++)
                {
                    if (ButtonArray[i] == true)
                    {
                        Lights[i].GetComponent<Light>().enabled = true;
                        LightCount++;
                    }
                }
                this.ButtonArray[0] = !this.ButtonArray[0];

            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Lights[1].GetComponent<Light>().enabled = !Lights[1].GetComponent<Light>().enabled;
               this.ButtonArray[1] = !this.ButtonArray[1];


            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Lights[2].GetComponent<Light>().enabled = !Lights[2].GetComponent<Light>().enabled;
               this.ButtonArray[2] = !this.ButtonArray[2];


            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
               this.ButtonArray[3] = !this.ButtonArray[3];
                //for(int i = 0; i < Lights.Length; i++){
                //    if(ButtonArray[i] == true){
                //        Lights[i].GetComponent<Light>().enabled = !Lights[i].GetComponent<Light>().enabled;
                //        LightCount++;

                //    }
                //}
               // Lights[3].GetComponent<Light>().enabled = !Lights[3].GetComponent<Light>().enabled;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                Lights[4].GetComponent<Light>().enabled = !Lights[4].GetComponent<Light>().enabled;
               this.ButtonArray[4] = !this.ButtonArray[4];

            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                Lights[5].GetComponent<Light>().enabled = !Lights[5].GetComponent<Light>().enabled;
               this.ButtonArray[5] = !this.ButtonArray[5];

            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                Lights[6].GetComponent<Light>().enabled = !Lights[6].GetComponent<Light>().enabled;
               this.ButtonArray[6] = !this.ButtonArray[6];

            }
            if (Input.GetKeyDown(KeyCode.Alpha8)) {
                Lights[7].GetComponent<Light>().enabled = !Lights[7].GetComponent<Light>().enabled;
               this.ButtonArray[7] = !this.ButtonArray[7];

            }
            if (Input.GetKeyDown(KeyCode.Alpha9)) {
                Lights[8].GetComponent<Light>().enabled = !Lights[8].GetComponent<Light>().enabled;
               this.ButtonArray[8] = !this.ButtonArray[8];

            }
            if(LightCount == 3){
                if (ButtonArray[2] == true && ButtonArray[3] == true && ButtonArray[7] == true){
                    misssion3++;
                    if(ButtonArray[0] == false && ButtonArray[1] == false && ButtonArray[4] == false && ButtonArray[5] == false && ButtonArray[6] == false && ButtonArray[8] == false && misssion3 == 1){
                    //complete mission3 back to base
                    //delay -> offlight -> delay -> white light
                    // for(int i = 0; i < Lights.Length; i++){
                    //    Lights[i].GetComponent<Light>().enabled = false;
                    //}
                     StartCoroutine(Complete(1));
                    print("complete Mission3");


                    LightCount = 999;
                    }


                }else{
                    // delay -> offlight -> delay -> redlight -> delay -> offlight
                    StartCoroutine(Incomplete(1));

                    print("incomplete");
                    LightCount = 0;
                }
            }

            if(Input.GetKeyDown(KeyCode.A)){
                this.Mission1Array[0] = true;
            }
            if(Input.GetKeyDown(KeyCode.S)){
                this.Mission1Array[1] = true;
            }
            //if(Input.GetKeyUp(KeyCode.A)){
            //    this.Mission1Array[0] = false;
            //}
            //if(Input.GetKeyUp(KeyCode.S)){
            //    this.Mission1Array[1] = false;
            //}
            if(Mission1Array[0] == true && Mission1Array[1] == true){
                Mission1Floor.GetComponent<MeshCollider>().enabled = true;
                this.MainDoor.SetActive(false);
                this.Mission1Array[0] = false;
                this.Mission1Array[1] = false;
                print("complete Mission1");
            }



            if (!this.Player1) {
                this.MapPlayer1();
            }
            if (!this.Player2) {
                this.MapPlayer2();
            }
            if (ButtonArray[0]) {
                this.MainDoor.SetActive(true);
                // Door ani
            }
            if (ButtonArray[1]) {
                // hihi
            }

            // check the player y position
            if (this.Player1 && this.Player1.transform.position.y < -16) {
                this.Player1.transform.position = this.P1point.transform.position;
            }
            if (this.Player2 && this.Player2.transform.position.y < -16) {
                this.Player2.transform.position = this.P2point.transform.position;
            }
        }
          IEnumerator Incomplete(float time)
            {
                yield return new WaitForSeconds(time);

                 for(int i = 0; i < Lights.Length; i++){
                        Lights[i].GetComponent<Light>().enabled = false;
                        Lights[i].color = Color.green;
                        ButtonArray[i] = false;
                    }

                yield return new WaitForSeconds(time);

                for(int i = 0; i < Lights.Length; i++){
                        Lights[i].GetComponent<Light>().enabled = true;
                        Lights[i].color = Color.red;
                    }

                yield return new WaitForSeconds(time);

                for(int i = 0; i < Lights.Length; i++){
                        Lights[i].GetComponent<Light>().enabled = false;
                        Lights[i].color = Color.green;
                        ButtonArray[i] = false;
                    }
                HintsCount++;

             }
             IEnumerator Complete(float time)
            {
                yield return new WaitForSeconds(time);

                 for(int i = 0; i < Lights.Length; i++){
                        Lights[i].GetComponent<Light>().enabled = false;
                    }

                yield return new WaitForSeconds(time);

                for(int i = 0; i < Lights.Length; i++){
                        Lights[i].color = Color.gray;
                        Lights[i].GetComponent<Light>().enabled = true;
                    }

                yield return new WaitForSeconds(time);

                for(int i = 0; i < Lights.Length; i++){
                        Lights[i].GetComponent<Light>().enabled = false;
                    }

                yield return new WaitForSeconds(time);

                for(int i = 0; i < Lights.Length; i++){
                    if(ButtonArray[i] == true){
                        Lights[i].color = Color.green;
                        Lights[i].GetComponent<Light>().enabled = true;
                    }
                }

                yield return new WaitForSeconds(time);

                missionManager.FinishMission(Constants.Mission.Stage1_1F);


             }


        void MapPlayer1() {
            GameObject Player1 = GameObject.FindWithTag("Player1Character");
            if (Player1 != null) {
                this.Player1 = Player1;
                this.Player1.transform.position = P1point.transform.position;
                ControlScript script = Player1.GetComponent("ControlScript") as ControlScript;
                if(script.getIsMe()) {
                    PlayerCamera cameraScirpt = FollowCamera.GetComponent("PlayerCamera") as PlayerCamera;
                    this.P1Script = script;
                    cameraScirpt.setCamera(this.Player1);
                }
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
                    this.P2Script = script;
                    cameraScirpt.setCamera(this.Player2);
                }
            }
        }

        
        public void ClickButton(int num) {
            //Lights[num].GetComponent<Light>().enabled = !Lights[num].GetComponent<Light>().enabled;
            this.ButtonArray[num] = !this.ButtonArray[num];
        }

        
        public void OnpressButton(int num){
            this.Mission1Array[num] = true;
        }
        
        public void UnpressButton(int num){
            this.Mission1Array[num] = false;
        }
    }
}
