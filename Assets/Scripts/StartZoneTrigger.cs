using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BarMovement bar = FindObjectOfType<BarMovement>();
            if (bar != null)
            {
                bar.CloseBar();
            }
        }
    }
}
