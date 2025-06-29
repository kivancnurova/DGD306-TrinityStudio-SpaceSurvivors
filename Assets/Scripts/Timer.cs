using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI timerText;
    public float elapsedTime = 0f;
    private bool isTimerRunning = false;

    public float interval = 10f;
    private float nextTrigger = 10f;

    public GameObject firstOption;



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
            EventSystem.current.SetSelectedGameObject(firstOption);
            nextTrigger += interval;
        }
        
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

}

