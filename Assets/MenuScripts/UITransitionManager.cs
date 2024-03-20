using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public GameObject crossfadeTransition;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // next scene in Build Settings
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        // decrease old camera priority
        currentCamera.Priority--;

        currentCamera = target;

        // increase target camera priority
        currentCamera.Priority++;
    }

    public void EnterGameCamera(CinemachineVirtualCamera target)
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        UpdateCamera(target);

        StartCoroutine(LoadMainLevel(target));
    }

    IEnumerator LoadMainLevel(CinemachineVirtualCamera target)
    {
        // reset dolly camera position
        target.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;

        // wait for cameras to switch
        yield return new WaitForSeconds(0.5f);

        // start moving dolly camera
        target.GetComponent<Animator>().enabled = true;
        
        crossfadeTransition.SetActive(true);
        crossfadeTransition.GetComponent<Animator>().enabled = true;

        // wait for dolly animation to play out
        yield return new WaitForSeconds(30f / 12f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayGame();
    }
}
