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
    [SerializeField] private int        gunLerpSpeed = 5;

    public int playerId = 0;

    private Player      player;
    private AudioSource audioS;
    
    // -- GUN ACTIONS
    private bool        fire;
    private bool        magAction;
    private bool        pullSlide;

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
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // all are working
        fire = player.GetButtonDown("Fire");
        magAction = player.GetButtonDown("MagAction");
        pullSlide = player.GetButtonDown("PullSlide");
        aim = player.GetButtonDown("Aim");
    }

    private void ProcessInput()
    {
        if (fire)
        {
            Shoot();
        }
        AimDownSigts();
        // etc
    }

    public void Shoot()
    {
        // debug & testing
        Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * 10, Color.red, 5.0f);
            Debug.Log("Hit " + hit.collider.name);
            if(hit.collider.name == "Target")
            {
                audioS.PlayOneShot(metalHit);
                Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    public void AimDownSigts()
    {
        if (aim)
        {
            // this variable helps lerp from current position when
            // pressing aim button rapidly. Always reset timer too.
            gunCurrentPos = pistol.transform.localPosition;
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
            pistol.transform.localPosition = Vector3.Lerp(gunCurrentPos, ADSpos, timer * gunLerpSpeed);
            timer += Time.deltaTime;
        }
        if (!isAiming)
        {
            pistol.transform.localPosition = Vector3.Lerp(gunCurrentPos, gunStartPos, timer * gunLerpSpeed);
            timer += Time.deltaTime;
        }
    }
}
