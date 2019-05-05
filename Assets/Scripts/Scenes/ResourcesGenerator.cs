using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.ResourcesGenerator {
    public class ResourcesGenerator : MonoBehaviour {
        #region Singleton Code
        static readonly ResourcesGenerator _instance = new ResourcesGenerator();
        public static ResourcesGenerator Instance {
            get { return _instance; }
        }
		#endregion

        public GameObject[] resourceList;
        public bool generated;
        public bool tutMode;
        public GameObject Tree, Rock, Metal, Ani;

        void Awake() {
        }

        void Start() {

        }

        void Update() {
        }

        public void randomGen() {
            if (!PhotonNetwork.isMasterClient && !tutMode) {
                return;
            }
            this.generated = true;
            Random.InitState(System.DateTime.Now.Second);
            foreach (GameObject p in this.resourceList) {
                int i = Random.Range(0, 7);
                if (i == 0 || i == 1) {
                    Debug.Log("-----------tree");
                    GameObject GO;
                    if (!tutMode) {
                        GO = PhotonNetwork.Instantiate("Tree", p.transform.position, p.transform.rotation, 0);
                    }
                    else {
                        GO = Instantiate(Tree, p.transform.position, p.transform.rotation);
                    }
                    Resources res = GO.GetComponent("Resources") as Resources;
                    res.setObj(GO);
                    res.setType("Tree");
                    GO.transform.parent = p.transform;
                }
                else if (i == 2 || i == 3) {
                    Debug.Log("-----------Rock");
                    GameObject GO;
                    if (!tutMode) {
                        GO = PhotonNetwork.Instantiate("Rock", p.transform.position, p.transform.rotation, 0);
                    }
                    else {
                        GO = Instantiate(Rock, p.transform.position, p.transform.rotation);
                    }
                    Resources res = GO.GetComponent("Resources") as Resources;
                    res.setObj(GO);
                    res.setType("Rock");
                    GO.transform.parent = p.transform;
                }
                else if (i == 4)
                {
                    Debug.Log("-----------Rock");
                    GameObject GO;
                    if (!tutMode)
                    {
                        GO = PhotonNetwork.Instantiate("Ani", p.transform.position, p.transform.rotation, 0);
                    }
                    else
                    {
                        GO = Instantiate(Ani, p.transform.position, p.transform.rotation);
                    }
                    Resources res = GO.GetComponent("Resources") as Resources;
                    res.setObj(GO);
                    res.setType("Ani");
                    GO.transform.parent = p.transform;
                }
                else
                {
                    Debug.Log("-----------metal");
                    GameObject GO;
                    if (!tutMode) {
                        GO = PhotonNetwork.Instantiate("Metal", p.transform.position, p.transform.rotation, 0);
                    }
                    else {
                        GO = Instantiate(Metal, p.transform.position, p.transform.rotation);
                    }
                    Resources res = GO.GetComponent("Resources") as Resources;
                    res.setObj(GO);
                    res.setType("Metal");
                    GO.transform.parent = p.transform;
                }
            }
        }

        public void reGen() {
            if (!PhotonNetwork.isMasterClient) {
                return;
            }
            this.generated = true;
            Random.InitState(System.DateTime.Now.Second);
            foreach (GameObject p in this.resourceList) {
                int i = Random.Range(0, 3);
                Resources res = p.GetComponent("Resources") as Resources;
                PhotonNetwork.Destroy(res.getObj());
                if (i == 0) {
                    Debug.Log("-----------tree");
                    GameObject re = PhotonNetwork.Instantiate("Tree", p.transform.position, p.transform.rotation, 0);
                    res.setObj(re);
                    res.setType("tree");
                    re.transform.parent = p.transform;
                }
                else if (i == 1) {
                    Debug.Log("-----------stone");
                    // GameObject re = PhotonNetwork.Instantiate("Stone", p.transform.position, p.transform.rotation, 0);
                    // res.setObj(re);
                    // res.setType("stone");
                    // re.transform.parent = p.transform;
                }
                else {
                    Debug.Log("-----------metal");
                    // GameObject re = PhotonNetwork.Instantiate("Metal", p.transform.position, p.transform.rotation, 0);
                    // res.setObj(re);
                    // res.setType("matel");
                    // re.transform.parent = p.transform;
                }
            }
        }
    }
}