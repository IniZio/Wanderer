using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
    public class WaterSoundDelay : MonoBehaviour {
        public AudioSource audioSource;
        public float delay;

        void Start() {
            audioSource.PlayDelayed(delay * 100);
        }
    }
}