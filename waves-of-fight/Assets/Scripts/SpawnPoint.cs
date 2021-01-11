using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    enum SpawnState{
        Can,
        Wait
    }
    SpawnState spawnState = SpawnState.Can;
    
    public GameObject unit;
    public List<GameObject> WayPoints;
    public Transform point;
    [Header("Spawn Setting")]
    public int unitCount = 5;
    public int timeSpawn = 12;

    private Vector3 startPoint;

    private void Awake() {
        startPoint = point.position;
        unit.GetComponent<AIController>().pointsPositions = WayPoints;
    }

    void Update()
    {
        if(spawnState == SpawnState.Can){
            spawnState = SpawnState.Wait;
            startPoint = point.position;
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn(){
        for(int i = 0; i < unitCount; i++){
            Instantiate(unit, startPoint, new Quaternion());
            startPoint.x += 1.5f;
        }

        yield return new WaitForSeconds(timeSpawn);
        spawnState = SpawnState.Can;
    }
}
