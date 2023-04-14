using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private AudioClip  magInsertAud, magEjectAud, insertBullet;
    [SerializeField] private GameObject go_bullet;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private GameObject player;

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
        MagCheck();
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
            Instantiate(go_bullet, bulletSpawn.transform, false);
            bulletInMag = true;
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEmpty = false;
        }
        else if (bullets < 1)
        {
            Destroy(go_bullet);
            bulletInMag = false;
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEmpty = true;
        }
    }

    // ----------------------------------
    // BULLETS in or out

    public void InsertBullet()
    {
        if (bullets < MAXBULLETS)
        {
            bullets += 1;
            Debug.Log(bullets);
            audioS.PlayOneShot(insertBullet);
            // play sounds
        }
        else
        {
            Debug.Log("max bullets");
        }
        // if mag full, play sound or show text to
        // let player know.
    }

    public void LostBullet()
    {
        Debug.Log("buttlets: " + bullets);
        bullets -= 1;
    }

    // -------------------------------------
    // MAG CHECK

    private void MagCheck()
    {
        if (bullets > 0 && bulletInMag == false)
        {
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEmpty = false;
        }
        else if (bullets < 1)
        {
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEmpty = true;
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
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEjected = true;
        }
    }

    private void InsertMag()
    {
        if (insertMag)
        {
            pistolAnim.SetBool("eject-mag", false);
            audioS.PlayOneShot(magInsertAud);
            magInGun = true;
            Pistol pistolScript = player.GetComponent<Pistol>();
            pistolScript.magEjected = false;
        }
    }
}
