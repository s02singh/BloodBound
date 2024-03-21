using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordController : MonoBehaviour
{
    private float specialModeCooldownStatus = 0.0f;
    [SerializeField] private float specialModeCooldownPeriod = 5.0f;
    PlayerController playerController;
    
    [SerializeField] private float baseDamage = 20;
    public float swordRange = 2f;
    // 0: normal0, 1: normal1, 2: normal2, 3: special0, 4: special1
    [SerializeField] private int swordMode = 0;
    // column 1: normal, column 2: special
    private List<List<float>> damageModTable = new List<List<float>> {
        new List<float> { 0,  0},
        new List<float> { 5,  0},
        new List<float> {10,  0},
        new List<float> {20, 20},
        new List<float> {30, 25},
        new List<float> {40, 30},
        new List<float> {50, 35},
        new List<float> {60, 40},
        new List<float> {70, 45},
        new List<float> {80, 50},
        new List<float> {90, 55}
    };

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private Transform  lightningSpawnPoint;


    void Start()
    {        
        swordMode = 0;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (specialModeCooldownStatus < specialModeCooldownPeriod)
        {
            specialModeCooldownStatus += Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("here in collision");
        // Check if the collision is with the dragon
        if (collision.gameObject.CompareTag("Dragon"))
        {
            // Damage the dragon
            DamageDragon(1, 0);
        }
    }

    // attackType: 0: normal, 1: heavy, 2: special, 3: dash
    public void Attack(int comboCounter, int attackType)
    {
        switch (attackType)
        {
            case 0:
                ExecuteNormalAttack(comboCounter, attackType);
                break;
            case 1:
                ExecuteNormalAttack(comboCounter, attackType);
                break;
            case 2:
                if (swordMode >= 3 && specialModeCooldownStatus >= specialModeCooldownPeriod)
                {
                    ExecuteSpecialAttack(comboCounter, attackType);
                    specialModeCooldownStatus = 0.0f;
                }
                else 
                {
                    ExecuteNormalAttack(comboCounter, attackType);
                }
                break;
            case 3:
                ExecuteNormalAttack(comboCounter, attackType);
                break;
        }
    }

    public void ExecuteNormalAttack(int comboCounter, int attackType)
    {      
        // Get sword position and attack direction
        Vector3 swordPosition = transform.parent.position;
        swordPosition.y += 1.0f; // added to make the colliders work a little better, instead of raycast coming out from the ground it comes out from the player's chest
        Vector3 attackDirection = transform.forward;
        
        // Perform raycast
        RaycastHit hit;
        if (Physics.Raycast(swordPosition, attackDirection, out hit, swordRange))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                DamageEnemy(hit.collider.gameObject, comboCounter, attackType);
            }
            else if (hit.collider.gameObject.CompareTag("Dragon"))
            {
                DamageDragon(comboCounter, attackType);
            }
           
        }

        Bounds swordBounds = GetComponent<Collider>().bounds;
        Collider[] dragonColliders = Physics.OverlapBox(swordBounds.center, swordBounds.extents, transform.rotation, LayerMask.GetMask("Dragon"));

        if (dragonColliders.Length > 0)
        {
            DamageDragon(comboCounter, attackType);
        }
    }

    private void DamageDragon(int comboCounter, int attackType)
    {
        int damage = (int)CalculateDamage(comboCounter, attackType);

        GameObject enemy = GameObject.Find("Dragon");
        DragonAI dragonAI = enemy.GetComponent<DragonAI>();
        if (dragonAI != null)
        {
            dragonAI.TakeDamage(damage);
            Debug.Log("Did Damage to " + enemy.name + " with " + damage + " damage");
        }
    }

    public void ExecuteSpecialAttack(int comboCounter, int attackType)
    {
        Vector3 swordPosition = transform.position;
        Vector3 attackDirection = transform.forward;

        Vector3 leftPoint = swordPosition + attackDirection * 5f - transform.right * 5f;  
        Vector3 rightPoint = swordPosition + attackDirection * 5f + transform.right * 5f;
        Vector3 center = swordPosition;

        GameObject lightning = Instantiate(
            lightningPrefab,
            lightningSpawnPoint.position, 
            Quaternion.Euler(90,0,0)
        );  

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] dragons = GameObject.FindGameObjectsWithTag("Dragon");

        GameObject[] objects = CreateCombinedArrayFrom(enemies, dragons);
        foreach (GameObject obj in objects)
        {
            // Calculate vector from apex point to object's position
            Vector3 vectorToObj = obj.transform.position - swordPosition;

            // Calculate angle between direction vector and vector to object
            float angle = Vector3.Angle(attackDirection, vectorToObj);

            // Check if object is within cone's angle
            if (angle <= 45f / 2f)
            {
                // Check if object is within maximum distance
                if (vectorToObj.magnitude <= swordRange)
                {
                    DamageEnemy(obj, comboCounter, attackType);
                }
            }
        }
    }

    private float CalculateDamage(int comboCounter, int attackType)
    {
        float damage = baseDamage * comboCounter;
        switch (attackType)
        {
            case 0:
            case 1:
                damage += damageModTable[swordMode][1];
                break;
            case 2:
                damage += damageModTable[swordMode][0];
                break;
            case 3:
                damage = 10f;
                break;
            default:
                return 0;
        }
        return damage;
    }

    private void DamageEnemy(GameObject enemy, int comboCounter, int attackType)
    {

        /**
         * The Enemy can be of 2 types: EnemyAI and DragonAI. So this script checks
         * for both types and calls the appropriate method to deal damage to the enemy.
        */

        int damage = (int)CalculateDamage(comboCounter, attackType);
        playerController.rage += damage / 10;

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(damage);
            Debug.Log("Did Damage to " + enemy.name + " with " + damage + " damage");
        }

        DragonAI dragonAI = enemy.GetComponent<DragonAI>();
        if (dragonAI != null)
        {
            dragonAI.TakeDamage(damage);
            Debug.Log("Did Damage to " + enemy.name + " with " + damage + " damage");
        }
    }


    // call once to upgrade the sword
    public void Upgrade()
    {
        swordMode++;
    }

    // call to reset the sword to base state
    public void Reset()
    {
        swordMode = 0;
    }

    // returns the cooldown status as a percentage (0-1)
    public float GetSpecialModeCooldownStatus()
    {
        return (specialModeCooldownPeriod - specialModeCooldownStatus)/specialModeCooldownPeriod;
    }





    public static T[] CreateCombinedArrayFrom< T >(T[] first, T[] second)
    {
        T[] result = new T[first.Length + second.Length];
        Array.Copy(first, 0, result, 0, first.Length);
        Array.Copy(second, 0, result, first.Length, second.Length);
        return result;
    }
}
