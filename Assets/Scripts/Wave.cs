using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Wave
{
    public string name;
    public GameObject[] enemyPrefabs;
    public int count = 5;
    public float spawnInterval = 1f;
    public Vector2 spawnXRange = new Vector2(-7f, 7f);
    public float spawnY = 6f;
}