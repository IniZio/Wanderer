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

        public GameObject[] list;
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}