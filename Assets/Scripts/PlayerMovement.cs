using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Transform PlatformSpawnPoint;
	public static float FloorHeight = 0f;
	public static float Speed = 20f;
	public GameObject EnemyWall;

	Rigidbody RB;
	float JumpSpeed = 40f;

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
		if (JumpStartPos != Vector2.zero && Input.GetMouseButton (0) && Vector2.Distance (JumpStartPos, Input.mousePosition) > 50f) {
			JumpEndPos = Input.mousePosition;
			Jump ();
			JumpStartPos = Vector2.zero;
		}
	}

	void FixedUpdate() {
		RB.velocity = new Vector3 (RB.velocity.x, RB.velocity.y, Speed);
	}

	void Jump() {
		RB.velocity = Vector3.forward * Speed;

		float AngleRad = Mathf.Atan2(JumpEndPos.y -JumpStartPos.y, JumpEndPos.x - JumpStartPos.x);
		float AngleDeg = (180 / Mathf.PI) * AngleRad;

		Vector3 angleVelocity = Quaternion.AngleAxis (AngleDeg, Vector3.left) * Vector3.forward;
		Vector3 forceVector = new Vector3 (angleVelocity.x * JumpSpeed, angleVelocity.y * JumpSpeed, Mathf.Clamp (angleVelocity.z * JumpSpeed, 0, JumpSpeed));
		RB.AddForce (forceVector, ForceMode.VelocityChange);
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "PlatformTop" && FloorHeight != col.transform.parent.position.y) {
			PlatformSpawnPoint.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				col.transform.parent.position.y,
				PlatformSpawnPoint.transform.position.z
			);
			FloorHeight = col.transform.parent.position.y;
		}
	}

	void OnEnable() {
		EnemyWall.GetComponent<GameObjectFollower> ().OffsetZ = -20f;
	}

	void OnCollisionEnter(Collision col) {
		// TODO only when front of the crate
		if (col.gameObject.tag == "Crate") {
			EnemyWall.GetComponent<GameObjectFollower> ().OffsetZ += 7f;
		}
		if (col.gameObject.tag == "Enemy") {
			gameObject.SetActive (false);
		}
	}
}
