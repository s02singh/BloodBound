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
        if (playerController.GetComponent<PlayerController>().currentHealth <= 0)
        {
            // prompt fade in
            transition.SetTrigger("Died");

            // Stop camera from moving
            playerController.GetComponent<ThirdPersonController>().LockCameraPosition = true;

            // Show cursor and let it move
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        /* Debugging
        // simulate game over
        if (Input.GetKeyDown(KeyCode.M))
        {
            // prompt fade in
            transition.SetTrigger("Died");

            // Stop camera from moving
            playerController.GetComponent<ThirdPersonController>().LockCameraPosition = true;

            // Show cursor and let it move
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
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
        */
    }

    public void Restart()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        // Hide and lock cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerController.GetComponent<ThirdPersonController>().LockCameraPosition = false;

        if (GameObject.FindWithTag("Dragon") == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // in gladiator scene
        } 
        else
        {
            SceneManager.LoadScene("GladiatorPit"); // in dragon scene, load prev
        }
    }

    public void MainMenu()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene("MainMenu");
    }
}
