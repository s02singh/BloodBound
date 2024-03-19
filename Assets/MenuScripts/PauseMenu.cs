using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public GameObject playerCamera;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        // Stop camera from moving
        playerCamera.GetComponent<ThirdPersonController>().enabled = false;

        // Show cursor and let it move
        // Cursor.lockState = CursorLockMode.None; // <- problem code
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        // Pause application with timeScale
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ContinueGame()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        // Let camera move again
        playerCamera.GetComponent<ThirdPersonController>().enabled = true;

        // Hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        // Resume game with normal timeScale
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // previous scene in Build Settings
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}