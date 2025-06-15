using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class WaveManager : MonoBehaviour
{
    public List<GameObject> waves = new List<GameObject>();
    public GameObject level2Panel;
    public GameObject level3Panel;
    public GameObject NextLevelButton;

    private List<EnemyGroupController> controllers = new List<EnemyGroupController>();
    private int currentIndex = 0;

    void Start()
    {
        if (level2Panel != null)
            level2Panel.SetActive(false);

        foreach (var waveGO in waves)
        {
            var ctrl = waveGO.GetComponent<EnemyGroupController>();
            if (ctrl != null)
            {
                controllers.Add(ctrl);
                ctrl.OnWaveCompleted += OnWaveCompleted;
                ctrl.StopWave();
            }
            waveGO.SetActive(false);
        }

        if (controllers.Count > 0)
            ActivateWave(0);
    }

    private void ActivateWave(int index)
    {
        currentIndex = index;
        var waveGO = waves[index];
        waveGO.SetActive(true);
        controllers[index].StartWave();
    }

    private void OnWaveCompleted()
    {
        controllers[currentIndex].StopWave();
        waves[currentIndex].SetActive(false);

        if (currentIndex == controllers.Count - 1)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "GameScene")
            {
                if (level2Panel != null)
                {
                    Time.timeScale = 0f;
                    level2Panel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(NextLevelButton);
                }

            }
            else if (sceneName == "Level2")
            {
                if (level3Panel != null)
                {
                    Time.timeScale = 0f;
                    level3Panel.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(NextLevelButton);
                }
            }
            else if (sceneName == "Level3")
            {
                ActivateWave(0);
            }
        }
        else
        {
            ActivateWave(currentIndex + 1);
        }
    }
}
