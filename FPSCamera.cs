using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private float moveSpeed        = 10f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private float clampValue = 10f;
    [SerializeField] private Camera cam;


    public int playerId = 0;

    private Player player;
    private CharacterController cc;
    private Vector3 moveVector;
    private Vector2 lookVector;
    private Vector3 currentVelocity;
    private Vector3 myRot;


    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        cc = GetComponent<CharacterController>();
        
    }

    private void Start()
    {

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
        cc.Move((transform.right * moveVector.x + transform.forward * moveVector.z )* moveSpeed * Time.deltaTime);

        // Mouse input
        // CLAMP rotation
        // rotate cam up and down

        myRot = cam.transform.rotation.eulerAngles;
        myRot.x = Mathf.Clamp(myRot.x, -clampValue, clampValue);
        cam.transform.rotation = Quaternion.Euler(myRot);
        Debug.Log(myRot.x);
        cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);
        // rotate player left and right.
        gameObject.transform.Rotate(Vector3.up * lookVector.x * mouseSensitivity * Time.deltaTime);
    }
}
