using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MovingManager : MonoBehaviour
{
    #region AI components
    private NavMeshAgent navMeshAgent;
    private NavMeshObstacle navMeshObstacle;
    private Rigidbody rbody;
    #endregion

    #region Flags
    public bool flagNavMeshAgent = true;
    public bool flagRotateToEnemy = false;
    public bool flagGoToEnemy = false;
    public bool flagGoToPoint = true;
    public bool flagStopMoving = false;

    #endregion

    public GameObject currentTarget;
    public float speedRotation;
    public float distance;
    public Quaternion slerp;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        rbody = GetComponent<Rigidbody>();

        StartCoroutine(WaitEnabledNavMeshAgent());
    }

    private IEnumerator WaitEnabledNavMeshAgent(){
        yield return new WaitUntil(() => navMeshAgent != null && navMeshAgent.enabled);
        GoToPoint(currentTarget);
    }

    private void FixedUpdate() {
        if(flagRotateToEnemy && tag != "Dead"){
            if(currentTarget == null || !flagStopMoving){
                flagRotateToEnemy = false;
                return;
            }
            if(transform.rotation.y < 0 && transform.rotation.y > 360){
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            slerp = Quaternion.Slerp(transform.rotation,
                                     Quaternion.LookRotation(currentTarget.transform.position - transform.position), speedRotation * Time.deltaTime);
            
            slerp.x = 0;
            slerp.z = 0;

            transform.rotation = slerp;
        }
    }

    public void GoToEnemy(GameObject enemy){
        currentTarget = enemy;

        if(!flagGoToEnemy){
            SwitchFlags();
        }else{
            flagStopMoving = false;
        }

        StartCoroutine(GoToTarget());
    }

    public void GoToPoint(GameObject point){
        currentTarget = point;

        if(!flagGoToPoint){
            SwitchFlags();
        }else{
            flagStopMoving = false;
        }

        if(navMeshAgent.enabled && currentTarget != null){
            navMeshAgent.SetDestination(currentTarget.transform.position);
        }
        StartCoroutine(GoToTarget());
    }

    private IEnumerator GoToTarget(){
        if(tag == "Dead"){
            flagRotateToEnemy = false;
            yield break; 
        }
        if(!navMeshAgent.enabled){
            SwitchNavMesh();
        }
        if(flagGoToPoint){
            navMeshAgent.SetDestination(currentTarget.transform.position);
        }

        while(true){
            yield return new WaitForSeconds(.4f);
            
            if(currentTarget != null){
                if(!flagGoToPoint && navMeshAgent.enabled){
                    navMeshAgent.SetDestination(currentTarget.transform.position);
                }
                distance = Vector3.Distance(transform.position, currentTarget.transform.position);
            }
            
            if(distance <= navMeshAgent.stoppingDistance + 0.5f){
                if(navMeshAgent.enabled){
                    SwitchNavMesh();
                }
                flagStopMoving = true;
                flagRotateToEnemy = true;
                break;
            }
        }
    }

    private void SwitchFlags(){
        flagStopMoving = false;
        flagGoToEnemy = !flagGoToEnemy;
        flagGoToPoint = !flagGoToPoint;
    }

    private void SwitchNavMesh(){
        if(!navMeshObstacle.enabled){
            navMeshAgent.enabled = false;
            flagStopMoving = true;
        }

        rbody.isKinematic = !rbody.isKinematic;
        rbody.useGravity = !rbody.useGravity;
        navMeshObstacle.enabled = !navMeshObstacle.enabled;

        if(!navMeshObstacle.enabled){
            navMeshAgent.enabled = true;
            flagStopMoving = false;
        }

        flagNavMeshAgent = navMeshAgent.enabled;
    }
}