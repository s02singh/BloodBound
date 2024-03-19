using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI healthAndStaminaText;
    public PlayerController characterScript;

    void Update()
    {
        if (characterScript != null)
        {
            int roundedHealth = Mathf.RoundToInt(characterScript.currentHealth);
            int roundedStamina = Mathf.RoundToInt(characterScript.currentStam);

            string displayText = "Health: " + roundedHealth.ToString() + "\nStam: " + roundedStamina.ToString();

            healthAndStaminaText.text = displayText;
        }
    }
}
