using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Transform PlatformSpawnPoint;

	Rigidbody RB;
	float Speed = 20f;
	float JumpSpeed = 40f;
	float LastPlatformHeight = 0f;
	float FallAcceleration = 3f;
	float FallTerminalVelocity = 10f;

	Vector2 JumpStartPos;
	Vector2 JumpEndPos;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {	
		if (Input.GetMouseButtonDown(0)) {
			JumpStartPos = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			JumpEndPos = Input.mousePosition;
			Jump ();
		}
	}

	void FixedUpdate() {
		RB.velocity = new Vector3 (RB.velocity.x, RB.velocity.y, Speed);
	}

	void Jump() {
		RB.velocity = Vector3.forward * Speed;

		// Get Angle in Radians
		float AngleRad = Mathf.Atan2(JumpEndPos.y -JumpStartPos.y, JumpEndPos.x - JumpStartPos.x);
		// Get Angle in Degrees
		float AngleDeg = (180 / Mathf.PI) * AngleRad;

		Vector3 angleVelocity = Quaternion.AngleAxis (AngleDeg, Vector3.left) * Vector3.forward;
		Vector3 forceVector = new Vector3 (angleVelocity.x * JumpSpeed, angleVelocity.y * JumpSpeed, Mathf.Clamp (angleVelocity.z * JumpSpeed, 0, JumpSpeed));
		RB.AddForce (forceVector, ForceMode.VelocityChange);

		//RB.AddForce (Vector3.up * JumpSpeed, ForceMode.VelocityChange);
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "PlatformTop" && LastPlatformHeight != col.transform.parent.position.y) {
			PlatformSpawnPoint.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				col.transform.parent.position.y,
				PlatformSpawnPoint.transform.position.z
			);
			LastPlatformHeight = col.transform.parent.position.y;
		}
	}
}
