using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	bool Destroying = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Player" && !Destroying) {
			Destroying = true;
			StartCoroutine (WaitAndDestruct ());
		} else if (col.gameObject.tag == "Enemy" && !Destroying) {
			Destroying = true;
			Destruct ();
		}
	}

	IEnumerator WaitAndDestruct() {
		yield return new WaitForSeconds(0.1f);
		Destruct ();
	}

	void Destruct() {
		gameObject.SetActive (false);
		Destroying = false;
	}
}
