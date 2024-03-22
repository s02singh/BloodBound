using UnityEngine;

public class BloodParticleDeleter : MonoBehaviour
{
    void Update()
    {
        if (GetComponent<ParticleSystem>().isStopped)
        {
            Destroy(gameObject);
        }
    }
}