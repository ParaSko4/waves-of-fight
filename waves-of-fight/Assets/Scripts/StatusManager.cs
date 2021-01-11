using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [Header("Build Setting")]
    [Space(3)]
    [Range(0, 100)]
    public float health;
    public float maxHealth;
    public float armor;
    public float maxArmor;
    [Range(5, 70)]
    public float attackDamage = 0;
    [Range(0.5f, 1.8f)]
    public float attackSpeed = 0;

    public int evadeChance = 0;
    public float accuracy = 0;
}
