using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    public static EndGameController Instance { get; private set; }

    public string player1Tag = "CoOpP1";
    public string player2Tag = "CoOpP2";

    public GameObject endPanel;
    public TMP_Text winnerText;
    public GameObject mainMenuButton;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (endPanel != null)
            endPanel.SetActive(false);
    }

    public void OnPlayerKilled(string victimTag)
    {
        if (gameEnded) return;

        string winnerTag = null;
        if (victimTag == player1Tag)
            winnerTag = player2Tag;
        else if (victimTag == player2Tag)
            winnerTag = player1Tag;
        else
        {
            Debug.LogWarning($"EndGameController: OnPlayerKilled çağrıldı, beklenmeyen victimTag: {victimTag}");
            return;
        }

        gameEnded = true;
        ShowEndPanel(winnerTag);

        GameObject otherShip = GameObject.FindWithTag(winnerTag);
        if (otherShip != null)
        {
            var shooting = otherShip.GetComponent<ShipAutoShooting>();
            if (shooting != null)
                shooting.enabled = false;
        }
    }

    private void ShowEndPanel(string winnerTag)
    {
        if (endPanel == null || winnerText == null)
        {
            Debug.LogWarning("EndGameController: endPanel veya winnerText referansı atanmamış!");
            return;
        }
        Time.timeScale = 0f;
        endPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(mainMenuButton);

        if (winnerTag == player1Tag)
            winnerText.text = "Player 1 Won!";
        else if (winnerTag == player2Tag)
            winnerText.text = "Player 2 Won!";
        else
            winnerText.text = "Unknown Winner";
    }


    public void LoadCoOpMenu()
    {
        SceneManager.LoadScene("CoOpMenu");
        Time.timeScale = 1f;
    }
}
