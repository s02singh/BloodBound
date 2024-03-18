using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZoneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject startWaves;
    [SerializeField] private GameObject blockPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BarMovement bar = FindObjectOfType<BarMovement>();
            if (bar != null)
            {
                bar.CloseBar();
            }
            startWaves.SetActive(true);
            blockPlayer.SetActive(true);
        }
    }
}
