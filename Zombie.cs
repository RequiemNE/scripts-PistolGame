using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Animator        anim;
    private NavMeshAgent    agent;
    private GameObject      player;
    
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
        
    }

    enum stance
    {
        idle,
        walk,
        bite
    }
}
