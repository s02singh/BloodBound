using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutscene : MonoBehaviour
{
    public DragonAI dragonController;
    public PlayerController playerController;
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
        if (dragonController.GetHealth() <= 0 && playerController.currentHealth > 0 || Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(waitShowCursor());

            transition.SetTrigger("Victory");
        }
    }

    IEnumerator waitShowCursor()
    {
        yield return new WaitForSeconds(5f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
