using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Transform  muzzle;
    [SerializeField] private Transform  caseExitPoint;
    [SerializeField] private AudioClip  metalHit, empty;
    [SerializeField] private GameObject impact;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject magazine;
    [SerializeField] private GameObject casing;
    [SerializeField] private int        gunLerpSpeed = 5;
    [SerializeField] private float      fireRate = 1f;

    public int  playerId    = 0;
    public bool magEjected  = false;

    private Player      player;
    private AudioSource audioS;
    private Animator    anim;
    
    // -- GUN ACTIONS
    private bool        fire;
    public  bool        canFire         = true;
    private bool        isAnimating     = false; // used to stop input during animations
    private bool        magAction;
    private bool        pullSlide;
    private bool        checkChamber;
    private bool        stanceUp;
    private bool        stanceDown;
    private bool        firing;

    // -- AIMING
    private bool        aim;
    private bool        isAiming = false;
    private float       timer = 0f;
    private Vector3     gunStartPos;
    private Vector3     gunCurrentPos;
    private Vector3     ADSpos = new Vector3(0, -0.118f, 0.287f);
    private Quaternion  gunStartRotation;

    // -- MAG & BULLETS
    public  bool magEmpty       = false;
    private bool lastBullet     = false;
    private bool chamberEmpty   = false;

    // -- STANCES
    public bool b_stanceUp     = false;
    public bool b_stancDown    = true;
    public bool b_Ads          = false;

    private void Start()
    {
        b_stanceUp = false;
        b_stancDown = true;
        b_Ads = false;
    }

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
        BulletInChamber();
        //ProcessInput();
        // this variable helps lerp from current position when aiming
        // to avoid jumping gun
        gunCurrentPos = pistol.transform.localPosition;
    }

    // Used LateUpdate as it allows to move transform while animating.
    // This means I can Lerp.
    private void LateUpdate()
    {
        if (!b_stanceUp)
        {
            AimDownSigts();
        }

        ProcessInput();
        ApplyRecoil();
    }

    private void GetInput()
    {
        // all are working
        fire            = player.GetButtonDown("Fire");
        magAction       = player.GetButtonDown("MagAction");
        pullSlide       = player.GetButtonDown("PullSlide");
        aim             = player.GetButtonDown("Aim");
        checkChamber    = player.GetButtonDown("CheckChamber");
        stanceUp        = player.GetButtonDown("StanceUp");
        stanceDown      = player.GetButtonDown("StanceDown");

    }

    private void ProcessInput()
    {   // stances dictate what buttons I can use.

        if (b_stancDown) //## Stance DOWN - weapon down
        {
            if (pullSlide)
            {
                PullSlide();
            }
            if (fire)
            {
                if (!firing)
                {
                    Shoot();
                }
            }
            if (checkChamber)
            {
                CheckChamber();
            }
            if (magAction)
            {
                EjectMag();
            }
            if (stanceUp)
            {
                ChangeStance("up");
            }
            if (stanceDown)
            {
                ChangeStance("down");
            }
        }
        
        if (b_stanceUp) //## Stance UP - weapon up
        {
            if (stanceUp)
            {
                ChangeStance("up");
            }
            if (stanceDown)
            {
                ChangeStance("down");
            }
        }
        if(b_Ads) //## Aim down sights
        {
            if (pullSlide)
            {
                PullSlide();
            }
            if (fire)
            {
                if (!firing)
                {
                    Shoot();
                }
            }
            if (checkChamber)
            {
                CheckChamber();
            }
        }
    }

    private void EjectMag()
    {
        Debug.Log("pressed E");
        Magazine mag = magazine.GetComponent<Magazine>();
        mag.ManipulateMag();
    }

    public void Shoot()
    {
        if (canFire)
        {
            if (!lastBullet)
            {
                EjectCasing();
                Firing();
            }
            else
            { // fire last bullet in chamber, then cant fire.
                EjectCasing();
                Firing();
                canFire = false;
                lastBullet = false;
            }
        }
        else
        {
            audioS.PlayOneShot(empty);
        }
    }

    private void Firing()
    {
        // debug & testing
        Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward);

        // code
        Magazine mag = magazine.GetComponent<Magazine>();
        mag.LostBullet();

        anim.SetBool("fire", true);
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(muzzle.transform.position, muzzle.transform.forward * 10, Color.red, 5.0f);
            Debug.Log("Hit " + hit.collider.name);

            //----- TO DO move this into Target.cs
            if (hit.collider.name == "Target")
            {
                audioS.PlayOneShot(metalHit);
                Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // ZOMBIE TEST
            if (hit.collider.tag == "Zombie")
            {
                // eventually ragdoll only when dead
                // shot to head == death
                Zom_Ragdoll zom = hit.collider.GetComponentInParent<Zom_Ragdoll>();
                zom.EnableRagdoll();
            }
        }

        // recoil
        //quaternion.lerp random range bwtween -5 to 5 in x and Y

        StartCoroutine("Recoil");
        StartCoroutine(PauseFire(1f));
    }

    public void AimDownSigts()
    {        
        if (aim)
        {
            FPSCamera cam_script = gameObject.GetComponent<FPSCamera>();
            // pressing aim button rapidly. Always reset timer too.            
            timer = 0;
            if (isAiming)
            {
                
                isAiming = false;
                cam_script.ChangeSpeed("ads-down");
            }
            else
            {
                isAiming = true;
                cam_script.ChangeSpeed("ads-up");
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

    // USED TO INSERT BULLET
    private void PullSlide()
    {
        Magazine mag = magazine.GetComponent<Magazine>();
        if (magEjected)
        {
            // insert boolet
            
            mag.InsertBullet();
        }
        else
        {
            anim.SetBool("slide-pull", true);
            mag.LostBullet();
            if (!magEmpty && !lastBullet && !canFire)
            {
                canFire = true;
            }
            else if (magEmpty && lastBullet)
            {
                //lastBullet = false;
                canFire = false;
            }
        }
    }

    private void CheckChamber()
    {
        anim.SetBool("check-chamber", true);
    }

    private void ChangeStance(string stance)
    {
        FPSCamera cam_script = gameObject.GetComponent<FPSCamera>();
        switch(stance)
        {
            case "up":
                anim.SetBool("stance-up", true);
                cam_script.ChangeSpeed("stance-up");
                break;
            case "down":
                anim.SetBool("stance-up", false);
                cam_script.ChangeSpeed("stance-down");
                break;
        }
    }

    private void EjectCasing()
    {
        GameObject clone;
        clone = Instantiate(casing, caseExitPoint.transform.position, caseExitPoint.transform.rotation);
        clone.GetComponent<Rigidbody>().AddRelativeForce(40, 20, 10, ForceMode.Force);
        clone.GetComponent<Rigidbody>().AddRelativeTorque(-30, 50, 0, ForceMode.Force);
    }
    bool rotating = false;
    private void ApplyRecoil()
    {
        //https://stackoverflow.com/questions/37586407/rotate-gameobject-over-time
        // https://answers.unity.com/questions/615125/c-rotate-object-over-time.html
    }


    //--------------------
    // MAGAZINE

    private void BulletInChamber()
    {
        if (magEmpty && !lastBullet && canFire)
        {
            lastBullet = true;
            Debug.Log("Hit last bullet");
        }

        else if (magEmpty && !lastBullet && canFire)
        {
            // if magazine is in, press R to pull slide and chamber round.
            // Can fire == true;
            // dont think i need this. Unless I do bulletInChamber.
            
        }

        if (chamberEmpty)
        {
            canFire = false;
        }
    }

    //--------------------
    // COROTUINES

    IEnumerator PauseFire(float pauseTime)
    {
        firing = true;
        yield return new WaitForSeconds(pauseTime);
        firing = false;
    }


    IEnumerator Recoil()
    {
        yield return null;

        int recoilX = Random.Range(-50, 50);
        int recoilY = Random.Range(-50, 50);

        pistol.transform.Rotate(new Vector3(pistol.transform.eulerAngles.z, (float)recoilX, (float)recoilY), Space.Self);
    }
}
