using Fyp.Game.Network;
using Fyp.Game.PlayerControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct Mission2
{

    public int nextWave;
}

public class DungeonMission : Photon.PunBehaviour, IPunObservable
{
    public Constants.Mission nextMission = Constants.Mission.Stage1_1F;
    public bool finalBoss = false;

    private NPCManager npcManager;
    private Mission2 mission2;
    private readonly float alphaFadeValue;
    private bool justFailed = false;

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(nextMission);
            //stream.SendNext(mission2);
            //stream.SendNext(justFailed);
            photonView.RPC("ForceUpdate", PhotonTargets.All, nextMission, mission2, justFailed);
        }
        else
        {
            //nextMission = (Constants.Mission)stream.ReceiveNext();
            //mission2 = (Mission2)stream.ReceiveNext();
            //justFailed = (bool)stream.ReceiveNext();
        }
    }

    [PunRPC]
    void ForceUpdate(Constants.Mission a, Mission2 b, bool c)
    {
        nextMission = a;
        mission2 = b;
        justFailed = c;
    }


    // Start is called before the first frame update
    
    void Start()
    {
        npcManager = new NPCManager();
        //StartMission();
    }


    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKey("p") && PhotonNetwork.isMasterClient)
        {
            npcManager.Spawn("Orc_Gnur_Boss", GameObject.FindGameObjectWithTag("BossSpawnPoint"));
        }
        photonView.RPC("ForceUpdate", PhotonTargets.All, nextMission, mission2, justFailed);
        if (justFailed)
        {
            justFailed = false;
            //StartCoroutine(_FailMission());
            NetworkChangeScene.AllPlayerChangeScene("BaseNew");
            GameObject.Find("Player1Character").GetComponent<ControlScript>().health = 60;
            GameObject.Find("Player2Character").GetComponent<ControlScript>().health = 60;
        }

        if (PhotonNetwork.isMasterClient)
        {
            switch (nextMission)
            {
                case Constants.Mission.Stage2_1F:
                    bool waveDone = npcManager.AllDead();

                    if (waveDone/* && mission2.nextWave == 2*/)
                    {
                        FinishMission();
                        break;
                    }

                    //if (waveDone)
                    //{
                        //mission2.nextWave++;
                        //GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

                        //npcManager.Spawn("Orc_Gnur", enemySpawnPoints[2]);
                        //npcManager.Spawn("Orc_Gnur", enemySpawnPoints[3]);
                        //npcManager.Spawn("Orc_Gnur", enemySpawnPoints[4]);
                    //}
                    break;
            }
        }
    }

    
    public void StartMission()
    {
        if (PhotonNetwork.isMasterClient)
        {
            switch (nextMission)
            {
                case Constants.Mission.Stage2_1F:
                    npcManager = new NPCManager();

                    GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
                    npcManager.Spawn("Orc_Gnur", enemySpawnPoints[0]);
                    npcManager.Spawn("Orc_Gnur", enemySpawnPoints[1]);
                    break;
            }
        }
    }

    
    public void StopMission()
    {
        switch (nextMission)
        {
            case Constants.Mission.Stage2_1F:
                foreach (GameObject enemy in npcManager.enemies)
                {
                    Destroy(enemy);
                }
                break;
        }
    }

    
    public void FinishMission()
    {
        nextMission += 1;
        NetworkChangeScene.AllPlayerChangeScene("BaseNew");
        //StartMission();
    }

    
    public void FailMission()
    {
        Debug.Log("Just failed mission " + nextMission);
        justFailed = true;
        StopMission();
        NetworkChangeScene.AllPlayerChangeScene("BaseNew");
        GameObject.Find("Player1Character").GetComponent<ControlScript>().health = 60;
        GameObject.Find("Player2Character").GetComponent<ControlScript>().health = 60;
    }

    
    IEnumerator _FailMission()
    {
        UnityEngine.UI.Image blackScreen = GameObject.Find("Death").GetComponent<UnityEngine.UI.Image>();

        blackScreen.color = Color.black;
        blackScreen.canvasRenderer.SetAlpha(0.0f);
        blackScreen.CrossFadeAlpha(1.0f, 3, false);

        yield return new WaitForSeconds(3);

        NetworkChangeScene.AllPlayerChangeScene("BaseNew");
        GameObject.Find("Player1Character").GetComponent<ControlScript>().health = 60;
        GameObject.Find("Player2Character").GetComponent<ControlScript>().health = 60;
        yield break;
    }

    
    public void FinishMission(Constants.Mission mission)
    {
        nextMission = mission + 1;
        if (nextMission >= Constants.Mission.Mission1_2F)
        {
            NetworkChangeScene.AllPlayerChangeScene("BaseNew");
        }
        //StartMission();
    }

    
    void Teleport()
    {
     //   GameObject.FindGameObjectWithTag("Player1Character").transform.position
    }
    
}
