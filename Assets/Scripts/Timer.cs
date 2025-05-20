using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI timerText;
    public GameObject marketPanel;
    public float elapsedTime = 0f;
    private bool isTimerRunning = false;

    public float interval = 10f;
    private float nextTrigger = 10f;



    void Awake()
    {
        if(upgradeManager == null)
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
        }
    }

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
            upgradeManager.ShowUpgradeOptions();
            nextTrigger += interval;
        }
        
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

}

