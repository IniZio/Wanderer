using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.ResourcesGenerator {
    public class Resources : Resources {
        int hp;
        int maxHp;
        bool isGen = false;
        bool collected = false;

        void Start() {
            this.hp = 3;
            this.maxHp = 3;
            this.isGen  = true;
            this.collected = false;
        }

        void attacked(int damage) {
            this.hp -= damage;
            if (this.hp < 0 ) {
                this.hp = 0;
            }
            if (this.hp == 0) {
            }
        }
    }
}