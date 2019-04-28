using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.ResourcesGenerator {
    public abstract class Resources : MonoBehaviour {
        public int hp;
        public int maxHp;
        public bool isGen = false;
        public bool collected = true;

        public void attacked(int i) {
            this.hp -= i;
        }
    }
}
