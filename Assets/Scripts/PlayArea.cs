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

	void OnTriggerExit(Collider other) {
		// Disable everything that leaves the trigger
		other.gameObject.SetActive(false);
	}
}
