using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public PlayerController characterScript; 

    void Update()
    {
        if (characterScript != null)
        {
            healthText.text = "Health: " + characterScript.currentHealth.ToString();
        }
    }
}
