using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MainMenuController : MonoBehaviour
{

    public GameObject firstOption;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(firstOption);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1f;
    }
}
