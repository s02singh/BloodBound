using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    private PlayerController playerController;

    public Image staminaBar;

    void Start()
    {
        playerController = transform.parent.GetComponent<HealthUIReferences>().characterScript;
    }

    void Update()
    {
        staminaBar.fillAmount = playerController.currentStam / playerController.maxStam;
    }
}
