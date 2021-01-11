using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    public GameObject globalObject;
    private GlobalData global;

    private StatusManager status;

    private void Awake() {
        global = globalObject.GetComponent<GlobalData>();

        status = GetComponent<StatusManager>();

        status.health = global.buildHealth;
        status.maxHealth = global.maxBuildHealth;
        status.armor = global.buildArmor;
        status.maxArmor = global.maxBuildArmor;
    }
    
    public float SetDamage(float damage){
        status.health = global.CalculateHealth(status.health, damage, status.armor);

        if(status.health <= 0){
            GetComponent<Rigidbody>().AddForce(Vector3.up * global.buildDeathForce, ForceMode.Impulse);
            Death();
        }

        return status.health;
    }

    private void Death(){
        Destroy(gameObject, global.buildDeathTime);
    }
}
