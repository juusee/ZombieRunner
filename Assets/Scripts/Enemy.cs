using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	GameObject Player;
	Rigidbody RB;
	float Speed;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
		Player = GameObject.FindGameObjectWithTag ("Player");
		Speed = Player.GetComponent<Rigidbody> ().velocity.z;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		float random = Random.value > 0.5 ? 3 : -2f;
		Speed = Player.GetComponent<Rigidbody> ().velocity.z + random;
		RB.velocity = new Vector3 (RB.velocity.x, RB.velocity.y, Speed);
	}
}
