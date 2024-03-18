using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCollisionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is triggered by the player and the player hasn't passed through yet
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Calculate the direction from the player to the collider's center
                Vector3 direction = transform.position - other.transform.position;
                direction.y = 0f; // Ensure the force is applied horizontally

                // Apply a force opposite to the player's movement direction
                playerRigidbody.AddForce(-direction.normalized * 10f, ForceMode.Impulse);
            }
        }
    }
}
