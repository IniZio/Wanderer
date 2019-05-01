using Fyp.Game.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Mission2
{
    public int nextWave;
}

public class DungeonMission : Photon.PunBehaviour
{
    public Constants.Mission nextMission = Constants.Mission.Stage1_1F;

    private NPCManager npcManager;
    private Mission2 mission2;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(nextMission);
            stream.SendNext(mission2);
        }
        else
        {
            nextMission = (Constants.Mission)stream.ReceiveNext();
            mission2 = (Mission2)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartMission();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            switch (nextMission)
            {
                case Constants.Mission.Stage2_1F:
                    bool waveDone = npcManager.AllDead();

                    if (waveDone && mission2.nextWave == 2)
                    {
                        FinishMission();
                        break;
                    }

                    if (waveDone)
                    {
                        mission2.nextWave++;
                        GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

                        npcManager.Spawn("Orc_Gnur", enemySpawnPoints[2]);
                        npcManager.Spawn("Orc_Gnur", enemySpawnPoints[3]);
                        npcManager.Spawn("Orc_Gnur", enemySpawnPoints[4]);
                    }
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
        //StartMission();
    }

    public void FinishMission(Constants.Mission mission)
    {
        nextMission = mission + 1;
        NetworkChangeScene.AllPlayerChangeScene("BaseNew");
        //StartMission();
    }

    void Teleport()
    {
     //   GameObject.FindGameObjectWithTag("Player1Character").transform.position
    }
}
