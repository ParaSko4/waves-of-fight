using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewManager : MonoBehaviour {
	public delegate void DelegateNotifyChanges(List<GameObject> newTarget, List<GameObject> oldTargets);
	public event DelegateNotifyChanges NotifyChanges;

	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;
	// [Layer]
	public LayerMask targetMask;
	// public LayerMask obstacleMask;

	[HideInInspector]
	public List<GameObject> visibleTargets = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> oldTargets;

	void Start() {
		StartCoroutine(FindTargetsWithDelay(.2f));
	}


	IEnumerator FindTargetsWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds(delay);
			if(transform.tag == "Dead"){
				visibleTargets.Clear();
				break;
			}
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets(){
		if(visibleTargets.Count != 0){
			oldTargets = new List<GameObject>(visibleTargets);
		}
		visibleTargets.Clear();
		
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (var target in targetsInViewRadius){
			Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
				float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

				if(target.tag == "Dead" || transform.tag == target.tag){
					continue;
				}

				visibleTargets.Add(target.gameObject);
				oldTargets.Remove(target.gameObject);
			}
        }

		NotifyChanges?.Invoke(visibleTargets ,oldTargets);
	}


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
		if (!angleIsGlobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}