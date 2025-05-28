using System.Collections;
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

public enum MovementType { Stationary, SideToSide, ZigZag}

public class EnemyGroupController : MonoBehaviour
{
    public List<EnemySpawnInfo> enemies = new List<EnemySpawnInfo>();

    void Start()
    {
        foreach (var info in enemies)
            Spawn(info);
    }

    void Spawn(EnemySpawnInfo info)
    {
        GameObject go = Instantiate(info.prefab, info.spawnPoint.position, Quaternion.identity);

        var entrance = go.AddComponent<OffScreenEntrance>();
        entrance.speed = info.speed * 1.5f;
        entrance.Initialize(info.targetPoint, () =>
        {
            AttachMovement(go.transform, info);
        });
    }

    void AttachMovement(Transform enemy, EnemySpawnInfo info)
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
}