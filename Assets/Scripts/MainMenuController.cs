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


    public void Level2Load()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1f;
    }

    public void Level3Load()
    {
        SceneManager.LoadScene("Level3");
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void LoadCoOpMenu()
    {
        SceneManager.LoadScene("CoOpMenu");
    }
}
