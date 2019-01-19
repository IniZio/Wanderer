using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackAnimal : MonoBehaviour {
    public float maxRange;
    public float attackRange;
    public int Speed;

    private NavMeshAgent agent;
    private Vector3 thisPos;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        int multiplier = 1;
        thisPos = transform.position;

        Collider[] collis = Physics.OverlapSphere(transform.position, maxRange);

        foreach(Collider colli in collis)
        {
            if (colli && colli.tag == "Player")
            {
                Vector3 runTo = transform.position + ((colli.transform.position - transform.position) * multiplier);
                float distance = Vector3.Distance(transform.position, colli.transform.position);

                if (distance > attackRange)
                {
                    // TODO: switch to attack state
                    return;
                }
            }
        }
    }
}
