using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public GameObject currentPanel, nextPanel;

    public void switchPanel()
    {
        nextPanel.SetActive(true);
        currentPanel.SetActive(false);
    }

    public void loadScene(int sceneNumber)
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
