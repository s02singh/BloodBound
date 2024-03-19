using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RespawnMenu : MonoBehaviour
{
    public Animator transition;
    public GameObject playerController;

    void Update()
    {
        // simulate game over
        if (Input.GetKeyDown(KeyCode.M))
        {
            // prompt fade in
            transition.SetTrigger("Died");

            // Stop camera from moving
            playerController.GetComponent<ThirdPersonController>().LockCameraPosition = true;

            // Show cursor and let it move
            Cursor.visible = true;
            // Cursor.lockState = CursorLockMode.None; // <- problem code
        }

        // simulate restarting
        if (Input.GetKeyDown(KeyCode.K))
        {
            Restart();
        }

        // simulate go back to menu
        if (Input.GetKeyDown(KeyCode.L))
        {
            MainMenu();
        }
    }

    public void Restart()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // previous scene in Build Settings
    }
}
