using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int score = 0;
    public int playerMaxHealth = 100;
    public int playerCurrentHealth = 100;
    public int playerAttackDamage = 10;
    public float playerMovementSpeed = 5f;
    public float playerBulletSpeed = 5f;
    public float playerFireRate = 1.2f;
}
