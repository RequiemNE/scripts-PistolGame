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
    private float   moveSpeed = 10f;


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
        switch (speed)
        {
            case "stance-up":
                moveSpeed = MOVE_SPEED;
                break;
            case "stance-down":
                moveSpeed = MOVE_SPEED - 3.0f;
                break;
            case "ads-up":
                moveSpeed = MOVE_SPEED - 6.0f;
                break;
            case "ads-down":
                moveSpeed = MOVE_SPEED - 3.0f;
                break;
        }
    }
}
