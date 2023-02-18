using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{

    // mag insert pos Vector3(0,-0.03,-0.005)
    // mag insert rot Vector3(9,0,0)
    // quaternion Quaternion(0.0784591213,0,0,0.996917367)
    // mag hold rot -30, 30, 0

    [SerializeField] int                magLerpSpeed = 5;
    [SerializeField] private AudioClip  magInsertAud, magEjectAud;

    private float       timer         = 0f;
    private float       timer2        = 0f;
    private bool        magInGun      = true;
    private bool        ejectMag      = false;
    private bool        insertMag     = false;
    private Animator    pistolAnim; //for accessing animator
    private AudioSource audioS;
    private Vector3     magInsertPos  = new Vector3(0f, -0.03f, -0.005f);
    private Vector3     magInstertRot = new Vector3(9f, 0f, 0f);
    private Vector3[]   magHoldPos    = new[] { 
        new Vector3(0f, -0.162f, -0.044f),
        new Vector3(-0.19f, 0.069f, -0.007f)
    };
    private Vector3     magHoldRot    = new Vector3(-30, 30, 0);

    // Start is called before the first frame update
    void Start()
    {
        pistolAnim  = GameObject.FindGameObjectWithTag("Pistol").GetComponent<Animator>();
        audioS      = GameObject.FindGameObjectWithTag("Pistol").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {

    }

    public void ManipulateMag()
    {
        Debug.Log("called ManipulateMag");
        if(magInGun)
        {
            ejectMag = true;
            EjectMag();
        }
        else
        {
            insertMag = true;
            InsertMag();
        }
    }

    // --------------------------------------
    // MAG ANIMATIONS

    private void EjectMag()
    {
        if (ejectMag)
        {
            pistolAnim.SetBool("eject-mag", true);
            audioS.PlayOneShot(magEjectAud);
            magInGun = false;
        }
    }

    private void InsertMag()
    {
        if (insertMag)
        {
            pistolAnim.SetBool("eject-mag", false);
            audioS.PlayOneShot(magInsertAud);
            magInGun = true;
        }
    }

    // ---------------------------------------
    // COROUTINES

    IEnumerator LerpMag(string point)
    {
        yield return null;
    }

}
