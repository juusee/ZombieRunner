using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	Rigidbody RB;
	float Speed = PlayerMovement.Speed;
	float AttackSpeed = 20f;
	float JumpSpeed = 40f;
	float FloorHeight;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		if (Random.value < 0.01) {
			Attack ();
		}
		if (RB.velocity.z < Speed) {
			RB.AddForce(new Vector3(0, -0.5f, 1f), ForceMode.VelocityChange);
		}
	}

	void Attack() {
		RB.AddForce (new Vector3(0, 0.8f, 1f) * AttackSpeed, ForceMode.VelocityChange);
	}

	void Jump() {
		RB.velocity = Vector3.one * 1f;
		RB.AddForce (new Vector3(0, 1f, 0.5f) * JumpSpeed, ForceMode.VelocityChange);
	}

	void Forward() {
		RB.AddForce (new Vector3(0, 0.5f, 1f) * JumpSpeed, ForceMode.VelocityChange);
	}

	void Down() {
		RB.velocity = new Vector3 (0, -0.5f, 0.5f);
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.tag == "EnemyWall") {
			RB.velocity = Vector3.zero;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "PlatformTop" && FloorHeight != col.transform.parent.position.y) {
			FloorHeight = col.transform.parent.position.y;
		}
		if (col.gameObject.tag == "EnemyWall") {
			RB.velocity = Vector3.zero;
		}
		if (col.gameObject.tag == "HoleTrigger") {
			if (PlayerMovement.FloorHeight > FloorHeight) {
				Jump ();
			} else if (PlayerMovement.FloorHeight == FloorHeight) {
				Forward ();
			} else {
				Down ();
			}
		}
	}
}
