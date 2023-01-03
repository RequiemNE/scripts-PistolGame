using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed        = 10f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Camera cam;


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


    void Update()
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
    }

    private void ProcessMovement()
    {
        // WASD input
        currentVelocity = cc.velocity;
        Vector3 move = new Vector3(moveVector.x, 0, moveVector.z);
        Vector3 directtion = transform.InverseTransformDirection(move);
        cc.Move(move * moveSpeed * Time.deltaTime);
        if(move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        

        // Mouse input
            // CLAMP rotation
        // rotate cam up and down
        cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);
        // rotate player left and right.
        gameObject.transform.Rotate(Vector3.up * lookVector.x * mouseSensitivity * Time.deltaTime);
    }
}
