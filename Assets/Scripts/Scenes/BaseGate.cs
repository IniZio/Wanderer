using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;

namespace Fyp.Game.UI {
    public class BaseGate : MonoBehaviour {
        void OnTriggerEnter (Collider col) {
            if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
                ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
                script.enterBaseGate();
            }
        }

        void OnTriggerExit(Collider col) {
            if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
                ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
                    script.exitBaseGate();
            }
        }
    }
}
