using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

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

    public bool isDashing;
    float dashTimer;

    public bool dashFinished;

    CharacterController characterController;
    ThirdPersonController thirdPersonController;

    //Equip-Unequip parameters
    [SerializeField]
    private GameObject sword;
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
            if (isAttacking || isDodging || isEquipping || isBlocking || isKicking)
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
        /*
        // Made a triangle
        Vector3 swordPosition = sword.transform.position;
        Vector3 attackDirection = sword.transform.forward;

        Vector3 leftPoint = swordPosition + attackDirection * 5f - sword.transform.right * 5f;  
        Vector3 rightPoint = swordPosition + attackDirection * 5f + sword.transform.right * 5f;
        Vector3 center = swordPosition;

        

        // Check for collisions within the box. need to add triangle logic later
        Collider[] hitColliders = Physics.OverlapBox(center, new Vector3(5f, 5f, 5f)); 
        
        lineRenderer.positionCount = 4;
        lineRenderer.SetPosition(0, swordPosition);
        lineRenderer.SetPosition(1, leftPoint);
        lineRenderer.SetPosition(2, rightPoint);
        lineRenderer.SetPosition(3, center);

        lineRenderer.startWidth = .1f;
        lineRenderer.endWidth = .05f;
        lineRenderer.enabled = true;

        foreach (Collider collider in hitColliders)
        {
            Debug.Log("Hit something: " + collider.gameObject.name);
            if (collider.gameObject.CompareTag("Enemy"))
            {
              
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    // WE NEED TO CHANGE THIS ONCE WE FIGURE OUT DAMAGE ENGINE
                    enemy.TakeDamage(100);

                }
            }
        }
        */
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

    // HOLD DOUBLE CLICK/RIGHT CLICK TO BLOCK
    private void Block()
    {
        if (Time.time < timeToBlock)
        {
            return;
        }

        if (isAttacking || isDodging || isEquipping || isKicking)
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
        if (isAttacking || !isEquipped)
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
            if (isEquipping)
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

        if (isAttacking || isDodging || isEquipping || isBlocking)
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
        if (isDodging)
        {
            return;
        }
        if (!isBlocking)
        {
            currentHealth -= damage;
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
        
      
    }



    // AT THE END OF ATTACKS. IGNORE THIS FOR NOW
    public void ResetAttack()
    {
        isAttacking = false;
    } 
}   