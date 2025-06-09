using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<GameObject> waves = new List<GameObject>();

    private List<EnemyGroupController> controllers = new List<EnemyGroupController>();
    private int currentIndex = 0;

    void Start()
    {
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

        int next = currentIndex + 1;
        if (next >= controllers.Count)
            next = 0;

        ActivateWave(next);
    }
}
