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
        // CLAMP -----------
        // try if statement
        // if cam.x. rotation > clampValue
        // cam.x rotation = clampValue

        //Debug.Log(cam.transform.localEulerAngles.x);
        //cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);

        float angle = Mathf.Repeat(cam.transform.localEulerAngles.x + 180, 360) - 180;

        if (angle >= 0f - clampValue &&
            angle <= clampValue)
        {
            cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);
        }
        else
        {
            Debug.Log("Outside loop");
            if (angle < 0f - clampValue)
            {
                Debug.Log("in loop");
                float newClamp = 0f - clampValue;
                // rotate is actually adding clamp value to the current value.
                // which is why it's jumping from -80 to -160. it's -80 x 2
                cam.transform.Rotate(clampValue, 0, 0, Space.Self);
            }
        }
        Debug.Log(angle);

        

        
        // END CLAMP ------------
        //Debug.Log(myRot.x);
        //cam.transform.Rotate(Vector3.left * lookVector.y * mouseSensitivity * Time.deltaTime);
        //Debug.Log("Z: " + cam.transform.rotation.z + " X: " + cam.transform.rotation.x + " Y: " + cam.transform.rotation.y);
        
        // rotate player left and right.
        gameObject.transform.Rotate(Vector3.up * lookVector.x * mouseSensitivity * Time.deltaTime);
    }
}
