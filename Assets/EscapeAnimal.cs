using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EscapeAnimal : MonoBehaviour {
    public float minRange;
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

        Collider[] collis = Physics.OverlapSphere(transform.position, minRange);

        foreach(Collider colli in collis)
        {
            if (colli && colli.tag == "Player")
            {
                // transform.position += Vector3.Lerp(thisPos, -colli.transform.position, Time.deltaTime * Speed);
                Vector3 runTo = transform.position + ((transform.position - colli.transform.position) * multiplier);
                float distance = Vector3.Distance(transform.position, colli.transform.position);
                if (distance < minRange) agent.SetDestination(runTo);
            }
        }
    }
}
