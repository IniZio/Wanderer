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
    public bool isMoaning = false;
    public int health = 20;

    private NavMeshAgent agent;
    private Vector3 thisPos;
    Animator animator;
    public bool isTut = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }

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
        if (isTut) {
            animator.SetBool("sit", true);
            return;
        }
        int multiplier = 1;
        thisPos = transform.position;

        Collider[] collis = Physics.OverlapSphere(transform.position, minRange);

        if (state != "harmed")
        {
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
        }

        switch(state)
        {
            case "escaping":
                animator.SetBool("walk", false);
                animator.SetBool("run", true);
                agent.speed = escapeSpeed;
                break;
            case "harmed":
                if (!animator.GetBool("hit") && isMoaning)
                {
                    isMoaning = false;
                    if (health <= 0)
                    {
                        health = 0;
                        animator.SetBool("die", true);
                        return;
                    }
                    state = "";
                }
                else
                {
                    isMoaning = true;
                    animator.SetBool("hit", true);
                }
                break;
            default:
                animator.SetBool("walk", true);
                animator.SetBool("run", false);
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

    public bool Dead()
    {
        return animator.GetBool("die");
    }

    public void Harmed(int damage = 10)
    {
        state = "harmed";
        health -= damage;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        if (isTut) {
            return new Vector3(0, 0, 0);
        }
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
