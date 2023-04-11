using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private AudioClip  magInsertAud, magEjectAud, insertBullet;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;

    private bool         magInGun      = true;
    private bool         ejectMag      = false;
    private bool         insertMag     = false;
    private Animator     pistolAnim; //for accessing animator
    private AudioSource  audioS;
    // bullets
    private bool         bulletInMag = false;
    private int          bullets;
    private const int    MAXBULLETS = 7;

    // Start is called before the first frame update
    void Start()
    {
        pistolAnim  = GameObject.FindGameObjectWithTag("Pistol").GetComponent<Animator>();
        audioS      = GameObject.FindGameObjectWithTag("Pistol").GetComponent<AudioSource>();
        // TESTING
        bullets = 4;
        CheckForBullets();
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

    private void CheckForBullets()
    {
        if (bullets > 0 && bulletInMag == false)
        {
            Instantiate(bullet, bulletSpawn.transform, false);
            bulletInMag = true;
        }
        else if (bullets < 1)
        {
            Destroy(bullet);
            bulletInMag = false;
        }
    }

    public void InsertBullet()
    {
        if (bullets <= MAXBULLETS)
        {
            bullets += 1;

            // play sounds
        }

        // if mag full, play sound or show text to
        // let player know.
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
