using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string enemyName = "Enemy";

    public GameObject prefab;
    public Sprite enemyIcon;

    [Header("Sağlık")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Hareket")]
    public float speed = 1f;
    public float minDistance = 2f;
    public float maxDistance = 5f;
    public float stopDistance = 2f;
    public int bloodMoneyAmount = 50;
}
