using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    private int currentWave = 0;
    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        while (currentWave < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWave]));

            currentWave++;

            yield return new WaitForSeconds(2f);
        }
        Debug.Log("Tüm dalgalar temizlendi!");
    }

    IEnumerator SpawnWave(Wave wave)
    {
        enemiesAlive = wave.count;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(wave.spawnInterval);
        }


        while (enemiesAlive > 0)
            yield return null;
    }

void SpawnEnemy(Wave wave)
{
    GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
    Vector3 spawnPos;
    float checkRadius = 0.5f;    // düşmanın collider yarıçapına göre ayarla
    int maxAttempts = 10;         // takılıp kalmaması için

    // Uygun bir pozisyon bulana kadar dener
    for (int i = 0; i < maxAttempts; i++)
    {
        float x = Random.Range(wave.spawnXRange.x, wave.spawnXRange.y);
        spawnPos = new Vector3(x, wave.spawnY, 0);

        // O noktada başka bir düşman collider’ı var mı diye bak
        Collider2D hit = Physics2D.OverlapCircle(spawnPos, checkRadius, LayerMask.GetMask("Enemy"));
        if (hit == null)
        {
            // boşsa spawn et
            GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);
            var e = go.GetComponent<Enemy>();
            if (e != null) e.onDestroyed += OnEnemyDestroyed;
            return;
        }
    }

    // eğer 10 defa denediysek hâlâ bulamadıysa, yine de spawn et (ya da tamamen iptal et)
    GameObject fallback = Instantiate(prefab, new Vector3(
        Random.Range(wave.spawnXRange.x, wave.spawnXRange.y),
        wave.spawnY, 0), Quaternion.identity);
    var ef = fallback.GetComponent<Enemy>();
    if (ef != null) ef.onDestroyed += OnEnemyDestroyed;
}


    void OnEnemyDestroyed()
    {
        enemiesAlive--;
    }
}