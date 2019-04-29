using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : Photon.PunBehaviour
{
    public string state = "";
    public float maxRange = 50;
    public float attackRange = 2;
    public int chaseSpeed = 5;
    public int normalSpeed = 2;
    public float swingInterval = 2;
    public float swingTimer = 0;
    public bool isSwinging = false;
    public bool isMoaning = false;
    public float wanderTimer = 0;
    public int health = 30;

    private GameObject target;
    private NavMeshAgent agent;
    Animator animator;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Re-validate target
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > maxRange)
            {
                target = null;
            }
        }

        // Searh for target
        if (target == null)
        {
            Collider[] collis = Physics.OverlapSphere(transform.position, maxRange);
            foreach (Collider colli in collis)
            {
                if (colli && colli.tag.IndexOf("Player") >= 0)
                {
                    target = colli.gameObject;
                    break;
                }
            }
        }

        if (state != "harmed")
        {
            if (Vector3.Distance(transform.position, target.transform.position) - 0.2 < attackRange)
            {
                animator.SetBool("Run", false);
                state = "attacking";
            }
            else
            {
                state = "chasing";
            }
        }

        switch (state)
        {
            case "attacking":
                animator.SetBool("Run", false);
                agent.isStopped = true;
                if (swingTimer < swingInterval)
                {
                    swingTimer += Time.deltaTime;
                    return;
                }
                swingTimer = 0;
                if (target == null)
                {
                    return;
                }
                transform.LookAt(target.transform);
                if (!animator.GetBool("Attack(1)") && isSwinging)
                {
                    isSwinging = false;
                    RaycastHit hit;
                    Vector3 fwd = transform.TransformDirection(Vector3.forward);

                    if (Physics.Raycast(transform.position, fwd, out hit, attackRange))
                    {
                        // TODO: actually deal damage
                    }
                }
                else
                {
                    isSwinging = true;
                    animator.SetBool("Attack(1)", true);
                }
                break;
            case "chasing":
                if (target == null)
                {
                    return;
                }
                Vector3 rangeVector = (target.transform.position - transform.position);
                Vector3 runTo = transform.position + rangeVector.normalized * (rangeVector.magnitude - attackRange);
                agent.SetDestination(runTo);
                animator.SetBool("Run", true);
                agent.isStopped = false;
                break;
            case "harmed":
                if (!animator.GetBool("Get_Hit") && isMoaning)
                {
                    isMoaning = false;
                    if (health <= 0)
                    {
                        animator.SetBool("Dead", true);
                        return;
                    }
                } else
                {
                    isMoaning = true;
                    animator.SetBool("Get_Hit", true);
                }
                break;
            default:
                agent.speed = normalSpeed;
                wanderTimer += Time.deltaTime;
                if (wanderTimer > 10)
                {
                    agent.SetDestination(RandomNavmeshLocation(60));
                    wanderTimer = 0;
                }
                break;
        }
    }

    public void Chase(GameObject target)
    {
        this.target = target;
        state = "chasing";
    }

    public void Attack(GameObject target)
    {
        this.target = target;
        state = "attacking";
    }

    public bool Dead()
    {
        return animator.GetBool("Dead");
    }

    public void Harmed(int damage = 10)
    {
        state = "harmed";
        health -= damage;
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

//if ((animator.GetCurrentAnimatorClipInfo(0).Length != 0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
  //      {
            //animator.enabled = false;
    //    }
      //  if (animator.GetCurrentAnimatorClipInfo(0).Length != 0)
        //{
      //      currAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip.;
        //}