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
    }
}
