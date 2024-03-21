using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public int damageAmount = 10; 
    public string enemyTag = "EnemyAI";

    private void OnParticleCollision(GameObject other)
    {
        // Check if the collided object has the specified tag
        if (other.CompareTag(enemyTag))
        {
            // Attempt to get the Health component of the collided object
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();

            // If the Health component exists, apply damage
            if (enemyAI != null)
            {
                enemyAI.TakeDamage(damageAmount);
            }
        }
    }
}
