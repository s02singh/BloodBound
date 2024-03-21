using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragonHealthBar : MonoBehaviour
{
    public DragonAI dragonController;

    public Image purpleHealthBar;
    public Image greenHealthBar;
    private Image currentHealthBar;

    private float maxHealth;

    void Start()
    {
        purpleHealthBar.enabled = true;
        greenHealthBar.enabled = false;

        currentHealthBar = purpleHealthBar;
        currentHealthBar.fillAmount = 1;

        maxHealth = dragonController.GetHealth();
    }

    void Update()
    {
        if (dragonController.GetStage2Status())
        {
            greenHealthBar.fillAmount = currentHealthBar.fillAmount;
            currentHealthBar.enabled = false;

            currentHealthBar = greenHealthBar;
            currentHealthBar.enabled = true;
        }

        currentHealthBar.fillAmount = dragonController.GetHealth() / maxHealth;
    }
}
