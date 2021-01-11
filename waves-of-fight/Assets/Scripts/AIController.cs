using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    #region External objects and parameters
    public GameObject globalObject;
    public List<GameObject> pointsPositions;
    #endregion

    private GlobalData global;

    #region AI components
    private StatusManager statusManager;
    private MovingManager moveManager;
    private ViewManager viewManager;
    private Rigidbody rbody;

    #endregion

    #region Fight methods
    [SerializeField]
    private bool fightStart = false;
    private List<GameObject> enemys = new List<GameObject>();
    private AIController enemyController;

    public void ReactionToVisibleEnemys(List<GameObject> newTargets, List<GameObject> oldTargets){
        foreach(var target in newTargets){
            if(enemys.Contains(target)){
                continue;
            }
            enemys.Add(target);
        }
        foreach (var target in oldTargets){
            if(enemys.Contains(target)){
                enemys.Remove(target);
            }
        }
        
        if(moveManager.flagStopMoving){
            if(!fightStart){
                if(moveManager.flagGoToEnemy && enemys.Count != 0 && moveManager.currentTarget != null){
                    enemyController = moveManager.currentTarget.GetComponent<AIController>();
                    if(!fightStart){
                        StartCoroutine(Attack());
                    }
                }else if(enemys.Count != 0){
                    moveManager.GoToEnemy(enemys[0]);
                }else if(pointsPositions.Count != 0) {
                    if(moveManager.flagGoToPoint){
                        pointsPositions.Remove(pointsPositions[0]);
                    }
                    moveManager.GoToPoint(pointsPositions[0].gameObject);
                }
            }else{
                if(moveManager.flagGoToEnemy && moveManager.flagStopMoving){
                    enemyController = moveManager.currentTarget.GetComponent<AIController>();
                    if(!fightStart){
                        StartCoroutine(Attack());
                    }
                }
            }
        }else{
            if(!moveManager.flagGoToEnemy && enemys.Count != 0){
                moveManager.GoToEnemy(enemys[0]);
            }
            //check, enemy live or die
            if(oldTargets.Contains(moveManager.currentTarget)){
                if(enemys.Count != 0){
                    moveManager.GoToEnemy(enemys[0]);
                }else if(pointsPositions.Count != 0){
                    moveManager.GoToPoint(pointsPositions[0].gameObject);
                }
            }
        }
    }

    public IEnumerator Attack(){
        fightStart = true;

        while (fightStart)
        {
            yield return new WaitForSeconds(statusManager.attackSpeed);

            if(enemyController != null && tag != "Dead"){
                if(enemyController.SetDamage(statusManager.attackDamage) <= 0){
                    if(enemys.Count != 0){
                        moveManager.GoToEnemy(enemys[0]);
                    }

                    enemyController = null;
                    fightStart = false;
                    break;
                }
                continue;
            }

            fightStart = false;
            break;
        }
    }

    private float SetDamage(float damage){
        if(statusManager.health > 0){
            statusManager.health = global.CalculateHealth(statusManager.health, damage, statusManager.armor);
            if(statusManager.health <= 0){
                transform.tag = "Dead";
            }

            rbody.AddForce(Vector3.up * global.unitForceAttack, ForceMode.Impulse);
            ChangeHealthCubeColor();
        }

        return statusManager.health;
    }
    #endregion

    #region Health detector and death effect
    private Material cubeHealthMaterial;

    private void ChangeHealthCubeColor(){
        if(statusManager.health <= 0){
            cubeHealthMaterial.SetColor("_Color" , Color.black);
            return;
        }
        if(statusManager.health <= statusManager.maxHealth * 25 / 100){
            cubeHealthMaterial.SetColor("_Color" , Color.red);
            return;
        }
        if(statusManager.health <= statusManager.maxHealth * 50 / 100){
            cubeHealthMaterial.SetColor("_Color" , Color.yellow);
            return;
        }
        if(statusManager.health <= statusManager.maxHealth * 75 / 100){
            cubeHealthMaterial.SetColor("_Color" , Color.green);
            return;
        }
    }

    private IEnumerator Death(){
        yield return new WaitUntil(() => transform.tag == "Dead");
        fightStart = false;

        Destroy(gameObject, global.unitDeathTime);
        rbody.AddForce(Vector3.up * global.unitDeathForce, ForceMode.Impulse);
    }
    #endregion

    private void Awake(){
        global = globalObject.GetComponent<GlobalData>();
        statusManager = GetComponent<StatusManager>();
        moveManager = GetComponent<MovingManager>();
        viewManager = GetComponent<ViewManager>();
        rbody = GetComponent<Rigidbody>();
        cubeHealthMaterial = transform.Find("HealthCube").GetComponent<Renderer>().material;

        statusManager.maxHealth = global.maxUnitHealth;
        statusManager.health = global.unitHealth;
        statusManager.maxArmor = global.maxBuildArmor;
        statusManager.armor = global.unitArmor;
        statusManager.attackDamage = global.unitMiliDamageAttack;
        statusManager.attackSpeed = global.unitMiliSpeedAttack;
        statusManager.evadeChance = global.evadeChance;
        statusManager.accuracy = global.accuracy;


        moveManager.speedRotation = global.speedRotation;
        moveManager.currentTarget = pointsPositions[0];

        viewManager.NotifyChanges += ReactionToVisibleEnemys;

        StartCoroutine(Death());
    }
}