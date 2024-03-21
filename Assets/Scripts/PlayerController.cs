using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using DigitalRuby.PyroParticles;

public class PlayerController : MonoBehaviour
{
    //Third Person Controller References
    [SerializeField]
    private Animator playerAnim;

    [SerializeField]
    AnimationCurve dodgeCurve;
    [SerializeField]
    AnimationCurve dashCurve;

    public bool isDodging;
    float dodgeTimer;

    public GameObject aura;
    public GameObject player;

    public GameObject[] slashes;

    public GameObject meteors;
    public float rage = 0f;

    public bool isDashing;
    float dashTimer;

    public GameObject dashVfx;
    public GameObject ultimateVFX;

    public bool dashFinished;

    public bool isMeteorUlt = false;

    CharacterController characterController;
    ThirdPersonController thirdPersonController;

    //Equip-Unequip parameters
    [SerializeField]
    public GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;

    public float maxHealth = 100f;
    public float currentHealth;
    public float regenCooldown = 2f;
    public float timeSinceHit = 0f;

    public float maxStam = 100f;
    public float currentStam;
    public float regenStam = 2f;
    public float timeSinceStam = 0f;

  
    private Vector3 dodgeDirection;


    //Blocking Parameters
    public bool isBlocking;

    //Kick Parameters
    public bool isKicking;

    //Attack Parameters
    public bool isAttacking;
    public float timeSinceAttack;
    public int currentAttack = 0;
    public float shieldDelay = 1f;
    public float timeToBlock = 0f;

    [SerializeField]
    LineRenderer lineRenderer;

    public AudioSource audioSource;
    public AudioClip swordSlash;
    public AudioClip rollSfx;


    private void Start()
    {
        Keyframe dodge_lastframe = dodgeCurve[dodgeCurve.length-1];
        dodgeTimer = dodge_lastframe.time;
        Keyframe dash_lastframe = dashCurve[dashCurve.length - 1];
        dashTimer = dash_lastframe.time;
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        currentStam = maxStam;

    }
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        // Call all mechanic functions
        DashAttack();
        MeteorShower();
        Attack();
        //HeavyAttack();
        Dodge();
        thirdPersonController.JumpAndGravity();
        Ultimate();
        Equip();
        Block();
        Kick();
        Regen();

    }

    private void Regen()
    {
        timeSinceHit += Time.deltaTime;
        if (timeSinceHit > regenCooldown && currentHealth < maxHealth)
        {
            currentHealth += 5 * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, 100);
        }

        timeSinceStam += Time.deltaTime;
        if (timeSinceStam > regenStam && currentStam < maxStam)
        {
            currentStam += 20 * Time.deltaTime;
            currentStam = Mathf.Clamp(currentStam, 0, 100);
        }
    }

    private void MakeSlash(int index)
    {
        slashes[index].SetActive(true);
    }

    private void DestroySlash(int index)
    {
        slashes[index].SetActive(false);
    }

    private void PlaySwordSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSlash);
        audioSource.pitch = 1f;
    }

    private void PlayRollSound()
    {
        audioSource.PlayOneShot(rollSfx);
    }

    // PRESS R TO EQUIP
    private void Equip()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded"))
        {
            if (isAttacking || isDodging || isEquipping || isBlocking || isKicking || isMeteorUlt)
                return;
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    // PRESS C TO ULTIMATE ABILITY
    private void Ultimate()
    {
        if (Input.GetKeyDown(KeyCode.C) && playerAnim.GetBool("Grounded"))
        {
            if (!isEquipped)
                return;
            isAttacking = true;
            playerAnim.SetTrigger("Ultimate");
        }
    }

    // CALLED BY ULTIMATE ANIMATION
    public void LightningStorm()
    {
        sword.GetComponent<SwordController>().Attack(currentAttack, 2);
    }
    

    // Called by Equip animation
    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            Debug.Log("here");
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
        }
    }

    // HELPER FUNCTION
    public void Equipped()
    {
        isEquipping = false;
    }

    private void DoDashVfx()
    {
        dashVfx.SetActive(true);
    }
    private void StopDashVfx()
    {
        dashVfx.SetActive(false);
    }
    // HOLD DOUBLE CLICK/RIGHT CLICK TO BLOCK
    private void Block()
    {
        if (Time.time < timeToBlock)
        {
            return;
        }

        if (isAttacking || isDodging || isEquipping || isKicking || isMeteorUlt)
            return;

        if (Input.GetKey(KeyCode.Mouse1) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            playerAnim.SetBool("Block", false);
            isBlocking = false;
        }
    }


    // Sprint + left click
    private void DashAttack()
    {
        if (isAttacking || !isEquipped ||isMeteorUlt)
            return;

        // Check for dodge input
        if (Input.GetMouseButtonDown(0) && !isAttacking && !isDodging && thirdPersonController._speed > 5)
        {
            if (isEquipping)
                return;
            isAttacking = false;
            if (!thirdPersonController.GroundedCheckPlayer())
                return;
            if (currentStam < 10f)
                return;
            currentStam -= 10f;
            timeSinceStam = 0;
            thirdPersonController._speed = 0;
            StartCoroutine(Dash());
        }
    }


    IEnumerator Dash()
    {
        playerAnim.SetTrigger("DashAttack");
        dashFinished = false;
        isDashing = true;
        float timer = 0;
        while (!dashFinished)
        {
            float speed = dashCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed);
            characterController.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDashing = false;



    }

    public void Rage()
    {
        if(rage < 100)
        {
            aura.SetActive(false);
        }
    }
    public void MeteorShower()
    {
        if (isAttacking || isDodging || isEquipping || isKicking || isBlocking ||isMeteorUlt)
            return;
        if (!thirdPersonController.GroundedCheckPlayer())
            return;
        if (rage == 100 && Input.GetKeyDown(KeyCode.V))
        {
            isMeteorUlt = true;
            MeteorSwarmScript meteorShowerScript = meteors.GetComponent<MeteorSwarmScript>();
            var meteorPosition = player.transform.position;
            meteorPosition.y += 18f;
            meteorShowerScript.Source = meteorPosition;
            playerAnim.SetTrigger("MeteorStrike");
            Instantiate(meteors, meteorPosition, Quaternion.identity);
  

        }
    }

    public void resetMeteor()
    {
        isMeteorUlt = false;
        rage = 0;
        aura.SetActive(false);
    }

    public void finishDash()
    {
        dashFinished = true;
        
    }
    // Press R to dodge
    private void Dodge()
    {
        
        // Check for dodge input
        if (Input.GetKeyDown(KeyCode.Q) && !isDodging && thirdPersonController._speed != 0)
        {
            if (isEquipping || isMeteorUlt)
                return;
            isAttacking = false;
            if (!thirdPersonController.GroundedCheckPlayer())
                return;
            if (currentStam < 10f)
                return;
            currentStam -= 10f;
            timeSinceStam = 0;
            StartCoroutine(Roll());
        }
    }


    IEnumerator Roll() {
        playerAnim.SetTrigger("Dodge");
        
        isDodging = true;
        float timer = 0;
        while (timer < dodgeTimer)
        {
            float speed = dodgeCurve.Evaluate(timer);
            Vector3 dir = (transform.forward * speed);
            characterController.Move(dir * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        isDodging = false;
        
            

    }

    // HOLD CTRL TO KICK
    public void Kick()
    {

        if (isAttacking || isDodging || isEquipping || isBlocking || isMeteorUlt)
            return;

        if (Input.GetKey(KeyCode.LeftControl) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            playerAnim.SetBool("Kick", false);
            isKicking = false;
        }
    }

    // PRESS Left Mouse Button TO ATTACK
    private void Attack()
    {

        if (isDodging || thirdPersonController.isJumping || isEquipping || isDashing)
            return;
        
        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            if (!isEquipped)
                return;

            if (!thirdPersonController.GroundedCheckPlayer())
                return;

            if (currentStam < 1f + (currentAttack + 1) * 1.2f)
                return;
            currentAttack++;
            isAttacking = true;

            

            // Check for chained heavy attack after the third attack
            if (currentAttack == 3 && Input.GetKey(KeyCode.F))
            {
                if (currentStam < 5f)
                    return;
                currentStam -= 5f;
                ChainedHeavyAttack();
                return;
            }

            if (currentAttack > 2)
                currentAttack = 1;

            //Reset
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            //Call Attack Triggers
            playerAnim.SetTrigger("Attack" + currentAttack);

            currentStam -= 1f + currentAttack * 1.2f;
            timeSinceStam = 0;

            //Reset Timer
            timeSinceAttack = 0;
        }
    }

    // Chained Heavy Attack triggered after the third light attack if F key is held
    private void ChainedHeavyAttack()
    {
        isAttacking = true;
        currentAttack = 0;
        playerAnim.SetTrigger("HeavyCombo");
        timeSinceAttack = 0;
    }

    private void PlayUltimateVFX()
    {
        ultimateVFX.SetActive(true);
    }
    private void StopUltimateVFX()
    {
        ultimateVFX.SetActive(false);
    }

    // PRESS Left Mouse Button + F TO HEAVY ATTACK
    private void HeavyAttack()
    {
        if (!isEquipped)
            return;

        if (Input.GetMouseButtonDown(0) && Input.GetKeyDown(KeyCode.F) && playerAnim.GetBool("Grounded"))
        {
            isAttacking = true;
            if (currentAttack == 3)
            {
                currentAttack = 1;
                playerAnim.SetTrigger("HeavyCombo");
                timeSinceAttack = 0;
            }
            else
            {
                Debug.Log("im pressed");
                playerAnim.SetBool("HeavyAttack1", true);
            }
        }
        else
        {
            Debug.Log("not pressed");
            playerAnim.SetBool("HeavyAttack1", false);
        }
    }




    // CALLED BY EACH ATTACK ANIMATION
    public void RaycastAttack()
    {
        
        if (!isEquipped)
        {
            return;
        }
        int attackCombo = currentAttack;
        if (attackCombo == 0)
        {
            attackCombo = 3;
        }
        int attackType = 0;
        if (isDashing)
            attackType = 3;
        sword.GetComponent<SwordController>().Attack(attackCombo, attackType);
        
    }

    // NO CHARACTER HEALTH YET. JUST ANIMATION
    public void TakeDamage(int damage)
    {
        if (isDodging || isMeteorUlt)
        {
            return;
        }
        if (!isBlocking)
        {
            currentHealth -= damage;
            rage += damage / 2;
            if (rage >= 100)
            {
                rage = 100;
                aura.SetActive(true);
            }
            timeSinceHit = 0f;
            playerAnim.SetTrigger("Hit");
        }
        else
        {
            Debug.Log("blocked");
            playerAnim.SetBool("Block", false);
            isBlocking = false;
            playerAnim.SetTrigger("BlockImpact");
            timeToBlock = Time.time + shieldDelay;
        }
        isDashing = false;
        isEquipping = false;
        isAttacking = false;
        isKicking = false;
        dashFinished = true;
        StopDashVfx();
        DestroyAllSlashes();
  
      
    }


    private void DestroyAllSlashes()
    {
        for (int i = 0; i < slashes.Length; i++)
        {
            DestroySlash(i);
        }
    }

    // AT THE END OF ATTACKS. IGNORE THIS FOR NOW
    public void ResetAttack()
    {
        isAttacking = false;
    } 
}   