using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    private List<GameObject> enemys;
    public List<GameObject> points;

    public GameObject curr;
    private MovingManager moveManager;
    private ViewManager viewManager;
    private bool flagMovingToPoint = false;

    private void Awake() {
        viewManager = GetComponent<ViewManager>();
        moveManager = GetComponent<MovingManager>();

        moveManager.speedRotation = 3;
        //moveManager.currentTarget = curr;

        viewManager.NotifyChanges += CheckMoveOldTargets;
    }

    public void CheckMoveOldTargets(List<GameObject> newTargets, List<GameObject> oldTargets){
        enemys = newTargets;

        // if(moveManager.flagStopMoving){
        //     if(flag)
        // }














        if(oldTargets != null && oldTargets.Contains(moveManager.currentTarget)){
            if(enemys.Count != 0){
                Debug.Log("go to enemy");
                moveManager.GoToEnemy(enemys[0]);
            }else{
                if(points != null && points.Count != 0){
                    moveManager.GoToPoint(points[0]);
                }else{
                    //moveManager.StopMoving();
                }
            }
        }else{
            if(!moveManager.flagGoToEnemy){
                moveManager.GoToEnemy(enemys[0]);
            }
        }
    }

    public bool RotateToEnemy(){
        if(flagMovingToPoint){
            
        }
        return !flagMovingToPoint;
    }
}
