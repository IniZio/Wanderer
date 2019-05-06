using Fyp.Game.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCControl : Photon.PunBehaviour, IPunObservable
{
    public string state = "";
    public float maxRange = 5;
    public float attackRange = 2;
    public int chaseSpeed = 5;
    public int normalSpeed = 2;
    public float swingInterval = 2;
    public float swingTimer = 0;
    public bool isSwinging = false;
    public bool isMoaning = false;
    public float moanTimer = 0;
    public float wanderTimer = 0;
    public int health = 15;
    private float deathTimer = 0;

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
        if (health <= 0)
        {
            health = 0;
            animator.SetBool("Dead", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Get_Hit", false);
            animator.SetBool("Attack(1)", false);
            return;
        }

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
                if (colli && (colli.tag.StartsWith("Player1Character") || colli.tag.StartsWith("Player2Character")))
                {
                    // a bit more random in target choosing
                    if (new System.Random().Next(2) == 0) {
                        target = colli.gameObject;
                    }
                    break;
                }
            }
        }

        if (state != "harmed")
        {
            if (target != null)
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
        }

        switch (state)
        {
            case "attacking":
                animator.SetBool("Run", false);
                animator.SetBool("Walk", false);
                agent.enabled = false;
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
                //if (!animator.GetBool("Attack(1)") && isSwinging)
                if (isSwinging)
                {
                    isSwinging = false;
                    RaycastHit hit;
                    Vector3 fwd = transform.TransformDirection(Vector3.forward);

                    if (Physics.Raycast(transform.position, fwd, out hit, attackRange + 1))
                    {
                        Debug.Log("Orc Hit something " + hit.collider.tag);
                        if (hit.collider.tag.StartsWith("Player1Character") || hit.collider.tag.StartsWith("Player2Character"))
                        {
                            Debug.Log("Orc sending harm call");
                            hit.collider.GetComponent<ControlScript>().Harmed(10);
                        }
                    } else
                    {
                        state = "";
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
                animator.SetBool("Walk", false);
                animator.SetBool("Attack(1)", false);
                Vector3 rangeVector = (target.transform.position - transform.position);
                Vector3 runTo = transform.position + rangeVector.normalized * (rangeVector.magnitude - attackRange);
                agent.SetDestination(runTo);
                animator.SetBool("Run", true);
                agent.enabled = true;
                break;
            case "harmed":
                moanTimer += Time.deltaTime;
                if (moanTimer >= 1 && isMoaning)
                {
                    isMoaning = false;
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack(1)", false);
                    animator.SetBool("Get_Hit", false);
                    animator.SetBool("Dead", true);
                    state = "";
                    return;
                } else
                {
                    isMoaning = true;
                    animator.SetBool("Walk", false);
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack(1)", false);
                    animator.SetBool("Get_Hit", true);
                }
                break;
            default:
                animator.SetBool("Attack(1)", false);
                animator.SetBool("Run", false);
                animator.SetBool("Walk", true);
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