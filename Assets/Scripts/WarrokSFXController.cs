using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarrokSFXController : MonoBehaviour
{

    public AudioSource audiosoruce;
    public AudioClip warrokHitSound;

    private void playWarrokHitSFX()

    {
        audiosoruce.PlayOneShot(warrokHitSound);
    }
}
