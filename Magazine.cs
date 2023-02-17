using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{

    // mag insert pos Vector3(0,-0.03,-0.005)
    // mag insert rot Vector3(9,0,0)
    // quaternion Quaternion(0.0784591213,0,0,0.996917367)
    // mag hold rot -30, 30, 0

    [SerializeField] int        magLerpSpeed = 5;
    [SerializeField] GameObject pistol; //for accessing animator

    private float     timer         = 0f;
    private float     timer2        = 0f;
    private bool      magInGun      = true;
    private bool      ejectMag      = false;
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

    }

    private void LateUpdate()
    {
        if (ejectMag)
        {
            if (gameObject.transform.localPosition == magInsertPos)
            {
                gameObject.transform.localPosition = Vector3.Lerp(magInsertPos, magHoldPos[0], timer * magLerpSpeed);
                timer += Time.deltaTime;
            }
            
            if (gameObject.transform.localPosition == magHoldPos[0])
            {
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[0], magHoldPos[1], timer2 * magLerpSpeed);
                timer2 += Time.deltaTime;
            }
            if (gameObject.transform.localPosition == magHoldPos[1])
            {
                //ejectMag = false;
            }
            //EjectMag();
        }
        /*
        if (insertMag)
        {
            if (gameObject.transform.localPosition == magInsertPos)
            {
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[1], magHoldPos[0], timer * magLerpSpeed);
                timer += Time.deltaTime;
            }

            if (gameObject.transform.localPosition == magHoldPos[0])
            {
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[0], magInsertPos, timer2 * magLerpSpeed);
                timer2 += Time.deltaTime;
            }
            if (gameObject.transform.localPosition == magInsertPos)
            {
                ejectMag = false;
            }
            InsertMag();
        }*/
    }

    public void ManipulateMag()
    {
        Debug.Log("called ManipulateMag");
        if(magInGun)
        {
            timer = 0f;
            timer2 = 0f;
            ejectMag = true;
        }
        else
        {
            timer = 0f;
            timer2 = 0f;
            insertMag = true;
        }
    }

    private void EjectMag()
    {
        // eject
        // lerp to magHoldPos[0]
        // if mag.transform == magHoldPos[0]
        // Lerp to magHoldPos[1]
        Debug.Log("in eject mag");

        if (gameObject.transform.localPosition == magInsertPos)
        {
            StartCoroutine(LerpMag("ej1"));
            //ejectMag = false;
        }
        if (gameObject.transform.localPosition == magHoldPos[0])
        {
            StartCoroutine(LerpMag("ej2"));
            magInGun = false;
        }
    }

    private void InsertMag()
    {
        // insert
    }

    // ---------------------------------------
    // COROUTINES

    IEnumerator LerpMag(string point)
    {
        Debug.Log("started lerp coroutine");
        timer = 0;
        switch (point)
        {
            case "ej1":

                gameObject.transform.localPosition = Vector3.Lerp(magInsertPos, magHoldPos[0], timer * magLerpSpeed);
                Debug.Log("started lerp case");
                timer += Time.deltaTime;
                yield return null;

                break;
            case "ej2":
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[0], magHoldPos[1], timer / magLerpSpeed);
                timer += Time.deltaTime;
                break;
            case "ins1":
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[1], magHoldPos[0], timer / magLerpSpeed);
                timer += Time.deltaTime;
                break;
            case "ins2":
                gameObject.transform.localPosition = Vector3.Lerp(magHoldPos[0], magInsertPos, timer / magLerpSpeed);
                timer += Time.deltaTime;
                break;
            default:
                Debug.Log("invalid case in Magazine.cs:LerpMag " + point);
                break;
        }
        yield return null;
    }

}
