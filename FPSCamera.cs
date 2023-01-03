using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed        = 10f;
    [SerializeField] private float mouseSensitivity = 10f;


    public int playerId = 0;

    private Player player;
    private CharacterController cc;
    private Vector3 moveVector;
    private Vector2 lookVector;
    private Vector3 currentVelocity;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        cc = GetComponent<CharacterController>();
    }


    void FixedUpdate()
    {
        GetMovement();
        ProcessMovement();
       // Debug.Log(move);
    }

    private void GetMovement()
    {
        moveVector.x = player.GetAxis("MoveHorizontal");
        moveVector.z = player.GetAxis("MoveForward");
        lookVector.x = player.GetAxis("LookHorizontal");
        lookVector.y = player.GetAxis("LookVertical");
        Debug.Log(moveVector.x);
    }

    private void ProcessMovement()
    {
        currentVelocity = cc.velocity;
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.z);
        cc.Move(move * moveSpeed * Time.deltaTime);
        //Debug.Log(move);
        //if (moveVector.x != 0.0f && moveVector.z != 0.0f)
        //{
        //   cc.Move(moveVector * moveSpeed * Time.deltaTime);            
        //}
    }
}
