using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Orc_Gnur_Controller : MonoBehaviour
{
    // Start is called before the first frame update

private UnityEngine.AI.NavMeshAgent navMeshAgent;
private Animator animator;
//public AudioClip[] chopSounds;
GameObject go;
AudioSource source;

void Start()
    {
navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
animator     = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
if(Input.GetKeyDown(KeyCode.Q)){
animator.SetBool("Walk", true);
}
if(Input.GetKeyUp(KeyCode.Q)){
animator.SetBool("Walk", false);
}
if(Input.GetKeyDown(KeyCode.W)){
animator.SetBool("Run", true);
}
if(Input.GetKeyUp(KeyCode.W)){
animator.SetBool("Run", false);
}
if(Input.GetKeyDown(KeyCode.E)){
JumpStart();
}
if(Input.GetKeyUp(KeyCode.E)){
 JumpStop();
}

if(Input.GetKeyDown(KeyCode.R)){
animator.SetBool("Idle_2", true);
}
if(Input.GetKeyUp(KeyCode.R)){
animator.SetBool("Idle_2", false);
}
if(Input.GetKeyDown(KeyCode.T)){
animator.SetBool("Idle_Other", true);
}
if(Input.GetKeyUp(KeyCode.T)){
animator.SetBool("Idle_Other", false);
}
if(Input.GetKeyDown(KeyCode.Y)){
animator.SetBool("Shout", true);
}
if(Input.GetKeyUp(KeyCode.Y)){
animator.SetBool("Shout", false);
}
if(Input.GetKeyDown(KeyCode.U)){
animator.SetBool("Get_Hit", true);
}
if(Input.GetKeyUp(KeyCode.U)){
animator.SetBool("Get_Hit", false);
}
if(Input.GetKeyDown(KeyCode.I)){
animator.SetBool("Attack(1)", true);
}
if(Input.GetKeyUp(KeyCode.I)){
animator.SetBool("Attack(1)", false);
}
if(Input.GetKeyDown(KeyCode.O)){
animator.SetBool("Attack(2)", true);
}
if(Input.GetKeyUp(KeyCode.O)){
animator.SetBool("Attack(2)", false);
}
if(Input.GetKeyDown(KeyCode.P)){
animator.SetBool("Attack(3)", true);
}
if(Input.GetKeyUp(KeyCode.P)){
animator.SetBool("Attack(3)", false);
}
if(Input.GetKey(KeyCode.H)){
animator.SetBool("Dead", true);
}
}
public void JumpStart(){

    animator.SetBool("Jump", true);
	go = new GameObject("Audio");
	source = go.AddComponent<AudioSource>();
	//source.clip = chopSounds[Random.Range(0,chopSounds.Length)];
   // source.PlayDelayed(0.6f);
    

}

public void JumpStop(){

source.Stop();
    animator.SetBool("Jump", false);
    Destroy(go, source.clip.length);

}

public AnimationClip GetAnimationClip(string name) {
     if (!animator) return null; // no animator
 
     foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
         if (clip.name == name) {
             return clip;
         }
 }
 return null;
 }

}
