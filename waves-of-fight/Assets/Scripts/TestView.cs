using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestView : MonoBehaviour
{
    [Layer]
    public int selectedLayer;
    private List<GameObject> enemys = new List<GameObject>();
    public string tagEnemy;
    public MovingManager moveManager;
    private ViewManager viewManager;

    private void Start() {
        viewManager = GetComponent<ViewManager>();
    }

    private void CheckViewList(){
        if (enemys.Count != 0)
        {
            List<GameObject> list = new List<GameObject>(enemys);            
            enemys = viewManager.visibleTargets;

            foreach (var target in viewManager.visibleTargets)
            {
                if(list.Contains(target)){
                    list.Remove(target);
                }
            }

            if(list.Contains(moveManager.currentTarget)){
                if(enemys.Count != 0){
                    moveManager.GoToEnemy(enemys[0]);
                }
                // else{
                //     moveManager.GoToPoint(points);
                // }
            }
        }else{
            enemys = viewManager.visibleTargets;

            // enemyRigidbody = enemys[0].GetComponent<Rigidbody>();
            // enemyController = enemys[0].GetComponent<AIController>();

            // flagMovingToPoint = false;
            // moveManager.ToTarget(enemys[0].transform);
        }
    }


    // private void OnTriggerEnter(Collider other) {
    //     if(other.gameObject.layer == selectedLayer && other.tag == tagEnemy){
    //         if(!enemys.Contains(other.gameObject)){
    //             enemys.Add(other.gameObject);
    //             Debug.Log(enemys.Count);

    //             moveManager.GoToEnemy(other.gameObject);

    //             Debug.Log("OnTriggerEnter");
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     if(other.gameObject.layer == selectedLayer && other.tag == tagEnemy){
    //         enemys.Remove(other.gameObject);
    //         Debug.Log(enemys.Count);

    //         Debug.Log("OnTriggerExit");
    //     }
    // }
}
