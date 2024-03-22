using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class LightningUIController : MonoBehaviour
{
    [SerializeField] private GameObject uiImage;

    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<HealthUIReferences>().characterScript;
        uiImage.GetComponent<UnityEngine.UI.Image>().fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float cooldownValue = playerController.timeSinceLightning / playerController.lightningCooldown;
        uiImage.GetComponent<UnityEngine.UI.Image>().fillAmount = cooldownValue;        
    }
}
