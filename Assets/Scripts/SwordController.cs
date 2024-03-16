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

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject swordElectricity;

    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private float lightningOffsetX = -0.03f;   // selected via testing
    [SerializeField] private float lightningOffsetZ =  2.54f;   // selected via testing
    [SerializeField] private float lightningOffsetY =  4.42f;   // selected via testing


    void Start()
    {        
        swordMode = 0;
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        
        swordElectricity.SetActive(false);
    }

    void Update()
    {
        if (swordMode >= 3) 
        {
            swordElectricity.SetActive(true);
        }
        else
        {
            swordElectricity.SetActive(false);
        }

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
            Debug.DrawRay(swordPosition, attackDirection * swordRange, Color.red, 3f);
            
            // lineRenderer.SetPosition(0, swordPosition);
            // lineRenderer.SetPosition(1, hit.point);
            // lineRenderer.startWidth = 0.1f;
            // lineRenderer.endWidth = 0.05f;
            // lineRenderer.enabled = true;

            if (hit.collider.gameObject.CompareTag("Enemy"))
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
            new Vector3(0,0,0), 
            new Quaternion(0.8509035f, 0, 0, 0.525322f)
        );

        lightning.GetComponent<LightningController>().Init(
            swordPosition.x+lightningOffsetX, 
            swordPosition.z+lightningOffsetZ, 
            swordPosition.y+lightningOffsetY
        );

        Collider[] hitColliders = Physics.OverlapBox(swordPosition, new Vector3(5f, 5f, 5f)); 
        
        // lineRenderer.positionCount = 4;
        // lineRenderer.SetPosition(0, swordPosition);
        // lineRenderer.SetPosition(1, leftPoint);
        // lineRenderer.SetPosition(2, rightPoint);
        // lineRenderer.SetPosition(3, center);
        // lineRenderer.startWidth = .1f;
        // lineRenderer.endWidth = .05f;
        // lineRenderer.enabled = true;

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                DamageEnemy(collider.gameObject, comboCounter, attackType);
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
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            int damage = (int)CalculateDamage(comboCounter, attackType);
            enemyAI.TakeDamage(damage);
            Debug.Log("Did Damage to " + enemy.name + " with " + damage + " damage");
        }
    }


    // call once to upgrade the sword
    public void Upgrade()
    {
        swordMode++;
    }

    // call to reset the sword to base state
    public void Reset() {
        swordMode = 0;
        swordElectricity.SetActive(false);
    }

    // returns the cooldown status as a percentage (0-1)
    public float GetSpecialModeCooldownStatus()
    {
        return (specialModeCooldownPeriod - specialModeCooldownStatus)/specialModeCooldownPeriod;
    }
}
