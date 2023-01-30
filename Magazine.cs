using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{

    // mag insert pos Vector3(0,-0.03,-0.005)
    // mag insert rot Vector3(9,0,0)
        // quaternion Quaternion(0.0784591213,0,0,0.996917367)
    // mag hold rot -30, 30, 0

    private float     timer         = 0f;
    private bool      magInGun      = true;
    private bool      ejectMag      = true;
    private bool      insertMag     = false;
    private Vector3   magInsertPos  = new Vector3(0f, -0.03f, -0.005f);
    private Vector3   magInstertRot = new Vector3(9f, 0f, 0f);
    private Vector3[] magHoldPos    = new[] { 
        new Vector3(0f, -0.162f, -0.044f),
        new Vector3(-0.19f, 0.069f, -0.007f)
    };
    private Vector3   magHoldRot    = new Vector3(-30, 30, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ejectMag)
        {
            EjectMag();
        }
        if(insertMag)
        {
            InsertMag();
        }
    }

    private void LateUpdate()
    {
        
    }

    public void ManipulateMag()
    {
        if(magInGun)
        {
            ejectMag = true;
        }
        else
        {
            insertMag = true;
        }
    }

    private void EjectMag()
    {
        // eject
        // lerp to magHoldPos[0]
            // if mag.transform == magHoldPos[0]
                // Lerp to magHoldPos[1]
      
    }

    private void InsertMag()
    {
        // insert
    }

}
