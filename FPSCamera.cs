using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private float MOVE_SPEED        = 10f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float clampValue       = 10f;
    [SerializeField] private Camera cam;


    public int playerId = 0;

    private Player player;
    private CharacterController cc;
    private Vector3 moveVector;
    private Vector2 lookVector;
    private Vector3 currentVelocity;
    private Vector3 myRot;
    private float   moveSpeed       = 10f;
    private float   stanceSpeed     = 0f;
    private float   directionSpeed  = 0f;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        cc = GetComponent<CharacterController>();
        moveSpeed = MOVE_SPEED;
    }

    void Update()
    {
        GetMovement();
        ProcessMovement();
        ChangeDirectionSpeed();
        // Update speed based on stance & direction
        moveSpeed = MOVE_SPEED - stanceSpeed - directionSpeed;
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
        cc.Move((transform.right * moveVector.x + transform.forward * moveVector.z )* moveSpeed * Time.deltaTime);

        // Rotate player up and down (uses cam.x)
        float angle = Mathf.Repeat(cam.transform.localEulerAngles.x + 180, 360) - 180;
        if (angle >= 0f - clampValue &&
            angle <= clampValue)
        {
            cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);
        }
        else
        {
            if (angle < 0f - clampValue)
            { 
                cam.transform.Rotate(2, 0, 0, Space.Self);
            }
            if (angle > clampValue)
            {
                cam.transform.Rotate(-2, 0, 0, Space.Self);
            }
        }
        // rotate player left and right.
        gameObject.transform.Rotate(Vector3.up * lookVector.x * mouseSensitivity * Time.deltaTime);
    }

    public void ChangeSpeed(string speed)
    {
        Pistol pistol = gameObject.GetComponent<Pistol>();
        switch (speed)
        {
            case "stance-up":
                stanceSpeed = 0;
                pistol.b_stanceUp   = true;
                pistol.b_stancDown  = false;
                pistol.b_Ads        = false;
                break;
            case "stance-down":
                stanceSpeed = 3f;
                pistol.b_stanceUp   = false;
                pistol.b_stancDown  = true;
                pistol.b_Ads        = false;
                break;
            case "ads-up":
                stanceSpeed = 5f;
                pistol.b_stanceUp    = false;
                pistol.b_stancDown   = false;
                pistol.b_Ads         = true;
                break;
            case "ads-down":
                stanceSpeed = 3f;
                pistol.b_stanceUp   = false;
                pistol.b_stancDown  = true;
                pistol.b_Ads        = false;
                break;
        }
    }

    private void ChangeDirectionSpeed()
    {
        // Can eventually add for bidirection (backwards and side)
        if (moveVector.x != 0)
        {
            directionSpeed = 2f;
        }
        if (moveVector.z < 0)
        {
            directionSpeed = 3f;
        }
        else
        {
            directionSpeed = 0f;
        }
    }
}
