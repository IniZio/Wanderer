using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fyp.Game.ResourcesGenerator;
using Fyp.Constant;

public class TutManager : MonoBehaviour {
    int tutState = 0;
    public ResourcesGenerator resGen;
    public GameObject player, dialog, gun, axe, wood, axePoint, gunPoint;
    public FadeInOut fadeInOut;
    public CanvasGroup uiElement;
    public ParticleSystem axeEffect, treeEffect, gunEffect, animalEffect, woodEffect;
    public Text content;
    public Image image;
    public GameObject ani;
    public AnimalControl ac;
    bool end = false;

    void Start() {
        resGen.randomGen();
        dialog.SetActive(false);
        fadeInOut.uiElement = uiElement;
        gun.SetActive(false);
        axe.SetActive(false);
        ani.SetActive(false);
        // wood.SetActive(true);
        axeEffect.Stop();
        treeEffect.Stop();
        gunEffect.Stop();
        // woodEffect.Stop();
        // animalEffect.Stop();
        setState(0);
    }

    void Update() {
        if (ac.GetState() == "dead" && !end) {
            setState(6);
            end = true;
        }
    }

    public void setState(int num) {
        switch (num) {
            case 0:
                content.text = GameConstant.DialogContent.STATE_0;
                showDialog();
                break;
            case 1:
                content.text = GameConstant.DialogContent.STATE_1;
                showDialog();
                axe.SetActive(true);
                axeEffect.Play();
                break;
            case 2:
                content.text = GameConstant.DialogContent.STATE_2;
                showDialog();
                axeEffect.Stop();
                axePoint.SetActive(false);
                treeEffect.Play();
                break;
            case 3:
                content.text = GameConstant.DialogContent.STATE_3;
                showDialog();
                treeEffect.Stop();
                ani.SetActive(true);
                // gunEffect.Play();
                // gun.SetActive(true);
                // woodEffect.Play();
                break;
            case 4:
                content.text = GameConstant.DialogContent.STATE_4;
                showDialog();
                // gunEffect.Stop();
                // woodEffect.Play();
                break;
            case 5:
                gunEffect.Stop();
                gunPoint.SetActive(false);
                ani.SetActive(true);
                break;
            case 6:
                content.text = GameConstant.DialogContent.STATE_5;
                showDialog();
                break;
        }
    }

    public void showDialog() {
        this.dialog.SetActive(true);
        fadeInOut.FadeIn();
    }

    public void hideDialog() {
        print("Fade Out");
        fadeInOut.FadeOut();
        this.dialog.SetActive(false);
        if (this.tutState == 0)
        {
            this.tutState += 1;
            this.setState(1);
        }
        // if (this.tutState == 3)
        // {
        //     this.tutState += 1;
        //     this.setState(4);
        // }
    }
}
