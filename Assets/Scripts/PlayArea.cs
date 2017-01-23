using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerExit(Collider col) {
		// Disable everything that leaves the trigger
		if (col.tag != "Enemy" && col.tag != "EnemyWall") {
			col.gameObject.SetActive(false);
		}
	}
}
