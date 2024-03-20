using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    [SerializeField] private float health;
    [SerializeField] private int damage;

    // Attacking
    private RaycastHit hit;
    [SerializeField] private float timeBetweenAttacks;
    private float timeSinceLastAttack;
    private float distanceToPlayer;
    [SerializeField] private float attackRange;
    [SerializeField] private bool playerInAttackRange, animated, alive, attacking;
    [SerializeField] private int rotationSpeed;

    [SerializeField] private Transform bloodSpawnPoint;
    [SerializeField] private GameObject bloodPrefab;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        if (animated)
        {
            animator = GetComponent<Animator>();
        }
        // Ready for attack
        timeSinceLastAttack = timeBetweenAttacks;
        attacking = false;
        alive = true;
    }


    private void Update()
    {
        // Make sure enemy is alive
        if (!alive)
        {
            return;
        }
        
        timeSinceLastAttack += Time.deltaTime;
        // Check for attack range
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Rotate look direction towards player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        if (distanceToPlayer < attackRange)
        {
            AttackPlayer();
        }

        if (distanceToPlayer >= attackRange)
        {
            FollowPlayer();
        }

        // TESTING DEATH ANIMATIONS // REMOVE WHEN DAMAGE IMPLEMENTED
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(100);
        }
    }

    private void FollowPlayer()
    {
        // Set enemy's destination to player's position
        if (!attacking)
        {
            agent.SetDestination(player.position);
        }
        if (animated)
        {
            animator.SetBool("inRange", false);
        }
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move when attacking player
        agent.SetDestination(transform.position);
        
        startAttack();
    }

    private void startAttack()
    {
        // Aim at enemy
        // Start attack (aiming at player) if not currently attacking for melee
        // Ranged characters can keep aiming at player while attacking
        if (attacking)
        {
            return;
        }

        if (animated)
        {
            animator.SetBool("inRange", true);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                animator.SetTrigger("attack");
                attacking = true;
            }
        }
    }
    private void RaycastHit()
    {
        // Raycast for melee enemies
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackRange))
        {

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                player.GetComponent<PlayerController>()?.TakeDamage(damage);
            }
        }
    }
    private void endAttack()
    {
        attacking = false;
        animator.SetBool("inRange", false);
        // Randomize attack patterns so all enemies dont attack at the same time
        timeSinceLastAttack = Random.Range(0f, 2f);
    }

    // TODO: DAMAGE IMPLEMENTATION
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            if (animated)
            {
                animator.SetBool("isAlive", false);
            }
            agent.enabled = false;
            alive = false;
            CapsuleCollider enemyCollider = GetComponent<CapsuleCollider>();
            enemyCollider.enabled = false;
            Destroy(gameObject, 5f);
        }

        GameObject b = Instantiate(
            bloodPrefab,
            bloodSpawnPoint.position, 
            Quaternion.identity
        );
        b.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
