using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 10; 

    private void OnParticleCollision(GameObject other)
    {
   
        if (other.CompareTag("Enemy"))
        {
           
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }
        }
        else if (other.CompareTag("Dragon"))
        {

            DragonAI dragonAI = other.GetComponent<DragonAI>();
            if (dragonAI != null)
            {
                dragonAI.TakeDamage(damageAmount);
            }
        }
    }
}
