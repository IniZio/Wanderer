using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalControl : Photon.PunBehaviour
{
    public string state = "";
    public float minRange = 10;
    public int normalSpeed = 2;
    public int escapeSpeed = 4;
    public float wanderTimer = 0;

    private NavMeshAgent agent;
    private Vector3 thisPos;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(RandomNavmeshLocation(100));
    }

    // Update is called once per frame
    void Update()
    {
        int multiplier = 1;
        thisPos = transform.position;

        Collider[] collis = Physics.OverlapSphere(transform.position, minRange);

        foreach (Collider colli in collis)
        {
            if (colli && colli.tag.IndexOf("Player") >= 0)
            {
                // transform.position += Vector3.Lerp(thisPos, -colli.transform.position, Time.deltaTime * Speed);
                Vector3 runTo = transform.position + ((transform.position - colli.transform.position) * multiplier);
                float distance = Vector3.Distance(transform.position, colli.transform.position);
                if (distance < minRange) agent.SetDestination(runTo);
                state = "escaping";
                break;
            }
        }

        switch(state)
        {
            case "escaping":
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                agent.speed = escapeSpeed;
                break;
            default:
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                agent.speed = normalSpeed;
                wanderTimer += Time.deltaTime;
                if (wanderTimer > 10)
                {
                    agent.SetDestination(RandomNavmeshLocation(100));
                    wanderTimer = 0;
                }
                break;
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
