using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager
{
    public List<GameObject> enemies = new List<GameObject>();

    public void Spawn(string name, GameObject point)
    {
        enemies.Add(PhotonNetwork.Instantiate(name, point.transform.position + new Vector3(1, 1.6f, 0), point.transform.rotation, 0));
    }

    public bool AllDead()
    {
        bool allDead = enemies.Count > 0;

        foreach (GameObject enemy in enemies)
        {
            if (!enemy.GetComponent<NPCControl>().Dead())
            {
                allDead = false;
            }
        }
        return allDead;
    }
}
