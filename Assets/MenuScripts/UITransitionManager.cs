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

        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;
    }

    public void EnterGameCamera(CinemachineVirtualCamera target)
    {
        // Deselects clicked button so that it is no longer selected.
        EventSystem.current.SetSelectedGameObject(null);

        UpdateCamera(target);
        target.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
        StartCoroutine(LoadLevel(target));
    }

    IEnumerator LoadLevel(CinemachineVirtualCamera target)
    {
        yield return new WaitForSeconds(0.5f);

        target.GetComponent<Animator>().enabled = true;
        crossfadeTransition.SetActive(true);
        crossfadeTransition.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(30f / 12f);

        PlayGame();
    }
}
