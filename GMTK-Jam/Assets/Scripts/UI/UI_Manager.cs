using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Image _LivesImg = null;
    [SerializeField]
    private Sprite[] _liveSprites = null;

    public void UpdateLives(int currentLives)
    {
        //give it a new one based on currentLives index
        _LivesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
