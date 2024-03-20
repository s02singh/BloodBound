using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningUlt : MonoBehaviour
{
    private PlayerController playerController;

    public Image ultBar;

    void Start()
    {
        playerController = transform.parent.GetComponent<HealthUIReferences>().characterScript;
        ultBar.fillAmount = 0f;
    }

    void Update()
    {
        // change later for ultimate cooldown
        ultBar.fillAmount += Time.deltaTime;
    }
}
