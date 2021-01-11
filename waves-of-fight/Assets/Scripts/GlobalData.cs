using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    [Header("Global Setting")]
    [Space(5)]
    [Header("General Parametrs")]
    public float oneArmorReduceDamage = 0.6f;
    public float maxAttackSpeed = 1.9f;
    [Space(3)]
    [Header("Unit Parametrs")]
    public float unitHealth = 100;
    public float unitArmor = 0;
    public float unitMiliSpeedAttack = 1.2f;
    public float unitMiliDamageAttack = 17.2f;
    [Space(1)]
    public float maxUnitHealth = 100;
    public float maxUnitArmor = 10;
    [Space(1)]
    public int evadeChance = 12;
    public float accuracy = 90;
    [Space(3)]
    [Header("Build Parametrs")]
    public float buildHealth = 100;
    public float buildArmor = 12;
    [Space(1)]
    public float maxBuildHealth = 100;
    public float maxBuildArmor = 30;
    [Space(4)]
    [Header("Customic Parametrs")]
    public float unitForceAttack = 2;
    public float unitDeathTime = 2;
    public float unitDeathForce = 20;
    public float speedRotation = 3;
    [Space(1)]
    public float buildDeathTime = 2;
    public float buildDeathForce = 20;
    
    private System.Random rd = new System.Random();
    
    public float CalculateHealth(float health, float damage, float armor){
        if(rd.Next(0, 100) <= evadeChance || rd.Next(0, 100) > accuracy){
            Debug.Log("attack fail!");
            return health;
        }

        damage -= armor * oneArmorReduceDamage;

        if(damage <= 0){
            return health;
        }
        return health - damage;
    }
}