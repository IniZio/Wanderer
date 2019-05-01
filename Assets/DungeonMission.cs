using Fyp.Game.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMission : Photon.PunBehaviour
{
    public Constants.Mission nextMission = Constants.Mission.Stage1_1F;

    private NPCManager npcManager;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(nextMission);
        }
        else
        {
            nextMission = (Constants.Mission)stream.ReceiveNext();
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
                    if (npcManager.AllDead())
                    {
                        FinishMission();
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

                    foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("EnemySpawnPoint"))
                    {
                        npcManager.Spawn("Orc_Gnur", spawn);
                    }
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
