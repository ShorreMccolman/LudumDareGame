using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarker : MonoBehaviour {

	public Transform target;

	void LateUpdate () {
		if(target != null) {
			transform.position = target.position + Vector3.up* 2.15f;
		}
	}
}
