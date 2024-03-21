using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffCliff : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerController.currentHealth = -1;
        }
    }
}
