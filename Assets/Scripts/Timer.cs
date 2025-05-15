using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject marketPanel;
    public float elapsedTime = 0f;
    private bool isTimerRunning = false;

    public float interval = 10f;
    private float nextTrigger = 10f;



    void Start()
    {
        isTimerRunning = true;
        marketPanel.SetActive(false);
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        if (elapsedTime >= nextTrigger)
        {
            TriggerMarketUI();
            nextTrigger += interval;
        }
        
    }


    private void TriggerMarketUI()
    {
        Time.timeScale = 0f;
        isTimerRunning = false;

        marketPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        marketPanel.SetActive(false);
        Time.timeScale = 1f;
        isTimerRunning = true;
    }

    

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

}

