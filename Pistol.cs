using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform  muzzle;
    [SerializeField] private AudioClip  metalHit;
    [SerializeField] private GameObject impact;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject magazine;
    [SerializeField] private int        gunLerpSpeed = 5;

    public int playerId = 0;

    private Player      player;
    private AudioSource audioS;
    private Animator   anim;
    
    // -- GUN ACTIONS
    private bool        fire;
    private bool        magAction;
    private bool        pullSlide;
    private bool        checkChamber;

    // -- AIMING
    private bool        aim;
    private bool        isAiming = false;
    private float       timer = 0f;
    private Vector3     gunStartPos;
    private Vector3     gunCurrentPos;
    private Vector3     ADSpos = new Vector3(0, -0.118f, 0.287f);
    private Quaternion  gunStartRotation;

    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        audioS = GetComponent<AudioSource>();
        gunStartPos = pistol.transform.localPosition;
        gunStartRotation = pistol.transform.localRotation;
        anim = pistol.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessInput();
        // this variable helps lerp from current position when aiming
        // to avoid jumping gun
        gunCurrentPos = pistol.transform.localPosition;
    }

    // Used LateUpdate as it allows to move transform while animating.
    // This means I can Lerp.
    private void LateUpdate()
    {
        AimDownSigts();
    }

    private void GetInput()
    {
        // all are working
        fire = player.GetButtonDown("Fire");
        magAction = player.GetButtonDown("MagAction");
        pullSlide = player.GetButtonDown("PullSlide");
        aim = player.GetButtonDown("Aim");
        checkChamber = player.GetButtonDown("CheckChamber");
    }

    private void ProcessInput()
    {
        if (fire)
        {
            Shoot();
        }
        if (pullSlide)
        {
            PullSlide();
        }
        if (checkChamber)
        {
            CheckChamber();
        }
        if (magAction)
        {
            EjectMag();
        }
        // etc
    }

    private void EjectMag()
    {
        Debug.Log("pressed E");
        Magazine mag = magazine.GetComponent<Magazine>();
        mag.ManipulateMag();
    }

    public void Shoot()
    {
        // debug & testing
        Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward);

        // code
        anim.SetBool("fire", true);
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * 10, Color.red, 5.0f);
            Debug.Log("Hit " + hit.collider.name);

            //----- TO DO move this into Target.cs
            if(hit.collider.name == "Target")
            {
                audioS.PlayOneShot(metalHit);
                Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        // recoil
        //quaternion.lerp random range bwtween -5 to 5 in x and Y
    }

    public void AimDownSigts()
    {
        
        if (aim)
        {
            // pressing aim button rapidly. Always reset timer too.            
            timer = 0;
            if (isAiming)
            {
                isAiming = false;
            }
            else
            {
                isAiming = true;
            }
        }
        if (isAiming)
        {
            pistol.transform.localPosition = Vector3.Lerp(gunCurrentPos, ADSpos, timer / gunLerpSpeed);
            timer += Time.deltaTime;
        }
        if (!isAiming)
        {
            pistol.transform.localPosition = Vector3.Lerp(gunCurrentPos, gunStartPos, timer / gunLerpSpeed);
            timer += Time.deltaTime;
        }
    }

    private void PullSlide()
    {
        anim.SetBool("slide-pull", true);
    }

    private void CheckChamber()
    {
        anim.SetBool("check-chamber", true);
    }
}
