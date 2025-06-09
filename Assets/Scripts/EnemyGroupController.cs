using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject prefab;
    public Transform spawnPoint;
    public Transform targetPoint;
    public MovementType movementType;
    public float entrySpeed = 4f;
    public float sideMinX = -2f;
    public float sideMaxX = +2f;
    public float speed = 2f;
    public float amplitude = 1f;
    public float frequency = 1f;
}

public enum MovementType { Stationary, SideToSide, ZigZag }

public class EnemyGroupController : MonoBehaviour
{
    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isRunning = false;

    public event Action OnWaveCompleted;

    public void StartWave()
    {
        if (isRunning) return;
        isRunning = true;

        foreach (var info in enemies)
            Spawn(info);

        if (activeEnemies.Count == 0)
            InvokeWaveCompleted();
    }

    public void StopWave()
    {
        foreach (var go in activeEnemies)
            if (go != null)
                Destroy(go);

        activeEnemies.Clear();
        isRunning = false;
    }

    private void Spawn(EnemySpawnInfo info)
    {
        GameObject go = Instantiate(info.prefab, info.spawnPoint.position, Quaternion.identity, transform);
        activeEnemies.Add(go);

        var entrance = go.AddComponent<OffScreenEntrance>();
        entrance.speed = info.speed * 1.5f;
        entrance.Initialize(info.targetPoint, () =>
        {
            AttachMovement(go.transform, info);
        });

        var notifier = go.AddComponent<DestroyNotifier>();
        notifier.onDestroyed = () =>
        {
            activeEnemies.Remove(go);
            if (activeEnemies.Count == 0)
                InvokeWaveCompleted();
        };
    }

    private void AttachMovement(Transform enemy, EnemySpawnInfo info)
    {
        switch (info.movementType)
        {
            case MovementType.Stationary:
                break;
            case MovementType.SideToSide:
                var sts = enemy.gameObject.AddComponent<SideToSideMovement>();
                sts.speed = info.speed;
                sts.minX = info.sideMinX;
                sts.maxX = info.sideMaxX;
                break;
            case MovementType.ZigZag:
                var zz = enemy.gameObject.AddComponent<ZigZagMovement>();
                zz.speed = info.speed;
                zz.amplitude = info.amplitude;
                zz.frequency = info.frequency;
                zz.Initialize(enemy);
                break;
        }
    }

    private void InvokeWaveCompleted()
    {
        OnWaveCompleted?.Invoke();
        Debug.Log($"{gameObject.name} tamamlandı.");
    }
}
