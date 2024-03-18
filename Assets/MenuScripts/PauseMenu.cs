using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject controlsMenu;
    [SerializeField]
    GameObject playerCamera;

    private bool isPaused = false;
    ThirdPersonController cameraController;

    // Upon pressing escape, open/close the pause menu.
    void Update()
    {
        // Escape key press - https://docs.unity3d.com/ScriptReference/KeyCode.Escape.html.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check the current state, and act based on if it is paused or resuming.
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

    public bool isGamePaused()
    {
        return isPaused;
    }

    public void PauseGame()
    {
        // Stop camera from moving
        cameraController = playerCamera.GetComponent<ThirdPersonController>();
        cameraController.enabled = false;

        // Show cursor and let it move
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        pauseMenu.SetActive(true);
        // Pause application with timeScale
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ContinueGame()
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        // Let camera move again
        cameraController = playerCamera.GetComponent<ThirdPersonController>();
        cameraController.enabled = true;

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