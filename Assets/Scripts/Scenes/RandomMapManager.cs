using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
    public class RandomMapManager : MonoBehaviour {
        public RandomMap.GameManager maze;

        void Start() {
        }

        void Update() {
            if (!maze.isGen && maze.isSyncSeed) {
                maze.BeginGame();
            }
        }
    }
}
