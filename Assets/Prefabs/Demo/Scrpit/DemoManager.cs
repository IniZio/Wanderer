using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    public GameObject[] playerList, animalList, enemyList, cameraList;
    public GameObject tree;
    public Transform[] positionList;

    // Start is called before the first frame update
    void Start()
    {


        // position1=GetComponent<>
        for (int i = 0; i < animalList.Length; i++)
        {
            animalList[i].transform.position = positionList[i].position;
            animalList[i].SetActive(false);
        }
        for (int i = 0; i < enemyList.Length; i++)
        {
            enemyList[i].transform.position = positionList[i].position;
            enemyList[i].SetActive(false);
        }
        for (int i = 0; i < playerList.Length; i++)
        {
            playerList[i].transform.position = positionList[i].position;
            playerList[i].SetActive(false);
        }
        cameraList[0].SetActive(true);
        cameraList[1].SetActive(false);
        tree.SetActive(false);
        playerList[2].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject obj in animalList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in enemyList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in playerList)
            {
                obj.SetActive(true);
            }
            tree.SetActive(false);
            playerList[2].SetActive(false);
            cameraList[0].SetActive(true);
            cameraList[1].SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject obj in playerList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in enemyList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in animalList)
            {
                obj.SetActive(true);
            }
            tree.SetActive(false);
            playerList[2].SetActive(false);
            cameraList[0].SetActive(true);
            cameraList[1].SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject obj in animalList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in playerList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in enemyList)
            {
                obj.SetActive(true);
            }
            tree.SetActive(false);
            playerList[2].SetActive(false);
            cameraList[0].SetActive(true);
            cameraList[1].SetActive(false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            foreach (GameObject obj in animalList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in enemyList)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in playerList)
            {
                obj.SetActive(false);
            }
            tree.SetActive(true);
            playerList[2].SetActive(true);
            cameraList[1].SetActive(true);
            cameraList[0].SetActive(false);
        }

    }
}
