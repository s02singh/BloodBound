using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerCollider : MonoBehaviour
{
    public int damageAmount = 10;
    public float damageCooldown = 0.1f; // Cooldown period in seconds
    private float lastDamageTime; // Time when the last damage was dealt

    private void Start()
    {
        lastDamageTime = -damageCooldown; // Initialize lastDamageTime to ensure damage can be applied immediately
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the triggering object is the player and if enough time has passed since the last damage
        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageCooldown)
        {
            // Apply damage to the player
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damageAmount);
                lastDamageTime = Time.time;
            }
        }
    }
}
