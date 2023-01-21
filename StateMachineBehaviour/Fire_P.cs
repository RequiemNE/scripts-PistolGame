using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_P : StateMachineBehaviour
{
    [SerializeField] private AudioClip fire;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("fire", false);
        animator.GetComponent<AudioSource>().PlayOneShot(fire);
    }
}
