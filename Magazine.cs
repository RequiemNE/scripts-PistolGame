using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private AudioClip  magInsertAud, magEjectAud;

    private bool        magInGun      = true;
    private bool        ejectMag      = false;
    private bool        insertMag     = false;
    private Animator    pistolAnim; //for accessing animator
    private AudioSource audioS;

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
}
