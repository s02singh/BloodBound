using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private PlayerController characterScript;

    private CanvasGroup canvasGroup;

    void Start()
    {
        characterScript = transform.parent.GetComponent<HealthUIReferences>().characterScript;
        
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    void Update()
    {        
        if (characterScript != null)
        {
            canvasGroup.alpha = (characterScript.maxHealth - characterScript.currentHealth) / characterScript.maxHealth;
        }
        else 
        {
            Debug.Log("HEALTH UI: Main Player not found");
        }
    }
}
