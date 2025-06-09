using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<GameObject> waves = new List<GameObject>();

    private int currentWave = 0;

    void Start()
    {
        for (int i = 0; i < waves.Count; i++)
            waves[i].SetActive(false);

        if (waves.Count > 0)
            ActivateWave(0);
    }

    void ActivateWave(int index)
    {
        currentWave = index;
        var waveGO = waves[index];
        waveGO.SetActive(true);

        var controller = waveGO.GetComponent<EnemyGroupController>();
        if (controller != null)
        {
            controller.OnWaveCompleted += () =>
            {
                StartNextWave();
            };
        }
        else
        {
            Debug.LogWarning($"[{waveGO.name}] üzerinde EnemyGroupController bulunamadı!");
            StartNextWave();
        }
    }

    void StartNextWave()
    {
        int next = currentWave + 1;
        if (next < waves.Count)
            ActivateWave(next);
        else
            Debug.Log("Tüm dalgalar tamamlandı!");
    }
}
