using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private float specialModeCooldownStatus = 0.0f;
    [SerializeField] private float specialModeCooldownPeriod = 5.0f;
    
    [SerializeField] private float baseDamage = 20;
    [SerializeField] private float swordRange = 15;
    // 0: normal0, 1: normal1, 2: normal2, 3: special0, 4: special1
    [SerializeField] private int swordMode = 0;
    // column 1: normal, column 2: special
    private List<List<float>> damageModTable = new List<List<float>> {
        new List<float> { 0,  0},
        new List<float> { 5,  0},
        new List<float> {10,  0},
        new List<float> {20, 30},
        new List<float> {30, 50}
    };

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private float lightningOffsetX = -0.03f;   // selected via testing
    [SerializeField] private float lightningOffsetZ =  2.54f;   // selected via testing
    [SerializeField] private float lightningOffsetY =  4.42f;   // selected via testing


    void Start()
    {        
        swordMode = 0;
    }

    void Update()
    {
        if (specialModeCooldownStatus < specialModeCooldownPeriod)
        {
            specialModeCooldownStatus += Time.deltaTime;
        }
    }

    // attackType: 0: normal, 1: heavy, 2: special
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
        }
    }

    public void ExecuteNormalAttack(int comboCounter, int attackType)
    {      
        // Get sword position and attack direction
        Vector3 swordPosition = transform.position;
        Vector3 attackDirection = transform.forward;

        // Perform raycast
        RaycastHit hit;
        if (Physics.Raycast(swordPosition, attackDirection, out hit, swordRange))
        {
            if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Dragon"))
            {
                DamageEnemy(hit.collider.gameObject, comboCounter, attackType);
            }
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
            new Vector3(swordPosition.x+lightningOffsetX,swordPosition.y+lightningOffsetY,swordPosition.z+lightningOffsetZ), 
            Quaternion.Euler(90,0,0)
        );

        lightning.GetComponent<LightningController>().Init(
            swordPosition.x+lightningOffsetX, 
            swordPosition.z+lightningOffsetZ, 
            swordPosition.y+lightningOffsetY
        );

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
        objects += GameObject.FindGameObjectsWithTag("Dragon");

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
}
