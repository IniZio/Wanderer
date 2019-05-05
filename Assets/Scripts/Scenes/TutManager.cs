using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fyp.Game.ResourcesGenerator;
using Fyp.Constant;

public class TutManager : MonoBehaviour {
    int tutState = 0;
    public ResourcesGenerator resGen;
    public GameObject player, dialog, gun, axe;
    public FadeInOut fadeInOut;
    public CanvasGroup uiElement;
    public ParticleSystem axeEffect, treeEffect, gunEffect, animalEffect;
    public Text content;
    public Image image;

    void Start() {
        resGen.randomGen();
        dialog.SetActive(false);
        fadeInOut.uiElement = uiElement;
        setState(0);
    }

    void Update() {
    }

    void setState(int num) {
        switch (num) {
            case 0:
                content.text = GameConstant.DialogContent.STATE_1;
                showDialog();
                break;
        }
    }

    void showDialog() {
        // this.dialog.SetActive(true);
        fadeInOut.FadeIn();
    }

    void hideDialog() {
        // this.dialog.SetActive(false);
        fadeInOut.FadeOut();
    }
}
