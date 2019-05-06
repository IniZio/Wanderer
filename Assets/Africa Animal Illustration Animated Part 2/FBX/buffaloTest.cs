using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffaloTest : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator     animator;
private bool _sit=false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator     = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)){
            animator.SetBool("eat", true);
        }
        if(Input.GetKeyUp(KeyCode.A)){
            animator.SetBool("eat", false);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            animator.SetBool("attack", true);
        }
        if(Input.GetKeyUp(KeyCode.S)){
            animator.SetBool("attack", false);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            animator.SetBool("hit", true);
        }
        if(Input.GetKeyUp(KeyCode.D)){
            animator.SetBool("hit", false);
        }
        if(Input.GetKey(KeyCode.H)){
            animator.SetBool("die", true);
        }
        if(Input.GetKey(KeyCode.F)&&!_sit){
            _sit=true;
            animator.SetTrigger("sit");
        }
        if(Input.GetKey(KeyCode.G)&&_sit){
            _sit=false;
            animator.SetTrigger("up");
        }
    }
}
