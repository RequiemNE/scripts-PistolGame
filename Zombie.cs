using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    enum stance
    {
        idle,
        walk,
        bite
    }

    private Animator        anim;
    private NavMeshAgent    agent;
    private GameObject      player;
    private stance stanceState = stance.idle;
    
    // Start is called before the first frame update
    void Start()
    {
        anim    = GetComponent<Animator>();
        agent   = GetComponent<NavMeshAgent>();
        player  = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // when moving play walk animation.
        switch(stanceState)
        {
            case stance.idle:
                agent.isStopped = true;
                anim.SetBool("idle", true);
                anim.SetBool("walk", false);
                break;
            case stance.walk:
                agent.isStopped = false;
                agent.destination = player.transform.position;
                anim.SetBool("walk", true);
                anim.SetBool("idle", false);
                break;
            case stance.bite:
                agent.isStopped = true;
                anim.SetBool("bite", true);
                anim.SetBool("walk", false);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            stanceState = stance.walk;
            Debug.Log("hit collider");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            stanceState = stance.idle;
            Debug.Log("out collider");
        }
    }
}
