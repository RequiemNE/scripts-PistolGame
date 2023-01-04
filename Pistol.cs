using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Pistol : MonoBehaviour
{
    public int playerId = 0;

    private Player  player;
    private bool    fire;
    private bool    magAction;
    private bool    pullSlide;
    private bool    aim;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // all are working
        fire = player.GetButtonDown("Fire");
        magAction = player.GetButtonDown("MagAction");
        pullSlide = player.GetButtonDown("PullSlide");
        aim = player.GetButtonDown("Aim");
    }

    private void ProcessInput()
    {
        // if fire
            //shoot()
        // if aim
            // aim()
        // etc
    }
}
