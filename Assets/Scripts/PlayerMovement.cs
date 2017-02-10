using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Transform PlatformSpawnPoint;
	public static float CurrentFloor = 0f;
	public static float MaxSpeed = 20f;
	public GameObject EnemyWall;

	Rigidbody RB;
	float JumpSpeed = 40f;

	Vector2 MoveStartPos;
	Vector2 MoveEndPos;

	enum PlayerState {Running, Jumping, Sliding, Stuck};
	PlayerState CurrentState;
	float SlidingStartPos = 0f;
	float SlidingLength = 20f;

	Quaternion TargetRotation;

	Vector3 LastObstaclePos;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// Development
		/*if (Input.GetKeyDown (KeyCode.UpArrow)) {
			CurrentFloor = 13f;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			CurrentFloor = 0f;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			CurrentFloor = -13f;
		}*/

		if (Input.GetMouseButtonDown(0)) {
			MoveStartPos = Input.mousePosition;
		}
		if (CurrentState == PlayerState.Running && MoveStartPos != Vector2.zero && Input.GetMouseButton (0) && Vector2.Distance (MoveStartPos, Input.mousePosition) > 50f) {
			MoveEndPos = Input.mousePosition;
			Move ();
			MoveStartPos = Vector2.zero;
		}
		float speed = RB.velocity.z;
		if (speed > MaxSpeed) {
			speed -= 30f * Time.deltaTime;
		} else {
			speed = MaxSpeed;
		}
		RB.velocity = new Vector3 (RB.velocity.x, RB.velocity.y, speed);
		if (CurrentState == PlayerState.Sliding && transform.position.z - SlidingStartPos > SlidingLength) {
			CurrentState = PlayerState.Running;
			TargetRotation = Quaternion.AngleAxis (0, Vector3.right);
		}

		float step = 500f * Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, step);
	}

	void Jump(float jumpAngle) {
		TargetRotation = Quaternion.AngleAxis (90 - jumpAngle, Vector3.right);
		Vector3 angleVelocity = Quaternion.AngleAxis (jumpAngle, Vector3.left) * Vector3.forward;
		Vector3 forceVector = new Vector3 (angleVelocity.x * JumpSpeed, angleVelocity.y * JumpSpeed, angleVelocity.z * MaxSpeed);
		RB.AddForce (forceVector, ForceMode.VelocityChange);
		CurrentState = PlayerState.Jumping;
	}
	
	void Slide() {
		CurrentState = PlayerState.Sliding;
		SlidingStartPos = transform.position.z;
		TargetRotation = Quaternion.AngleAxis (-80, Vector3.right);
	}

	void Move() {
		float moveAngle = Mathf.Atan2(MoveEndPos.y -MoveStartPos.y, MoveEndPos.x - MoveStartPos.x) * Mathf.Rad2Deg;

		if (moveAngle > 0 && moveAngle < 90) {
			Jump (moveAngle);
		} else if (moveAngle < 0) {
			Slide ();
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "PlatformTop" && CurrentFloor != col.transform.parent.position.y) {
			PlatformSpawnPoint.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				col.transform.parent.position.y,
				PlatformSpawnPoint.transform.position.z
			);
			CurrentFloor = col.transform.parent.position.y;
		}
		// Stuck on front of platform
		// TODO maybe add some injury animation and let zombies closer?
		if (col.gameObject.tag == "PlatformFront") {
			float rotation = transform.position.y > col.transform.position.y ? 60 : -60;
			TargetRotation = Quaternion.AngleAxis(rotation, Vector3.right);
			CurrentState = PlayerState.Stuck;
		}
		if (col.gameObject.tag == "PlatformTop" && CurrentState != PlayerState.Sliding && CurrentState != PlayerState.Stuck) {
			CurrentState = PlayerState.Running;
			TargetRotation = Quaternion.AngleAxis (0, Vector3.right);
		}
		// Position check prevents double trigger on same obstacle
		if (col.gameObject.tag == "Obstacle" && LastObstaclePos != col.transform.position) {
			EnemyWall.GetComponent<EnemyWallMovement> ().OffsetZ += 10f;
			LastObstaclePos = col.transform.position;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "PlatformFront") {
			CurrentState = PlayerState.Running;
		}
	}

	void OnEnable() {
		CurrentState = PlayerState.Running;
		EnemyWall.GetComponent<EnemyWallMovement> ().OffsetZ = -20f;
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "Enemy") {
			gameObject.SetActive (false);
		}
		// Position check prevents double trigger on same obstacle
		if (col.gameObject.tag == "Obstacle" && LastObstaclePos != col.transform.position) {
			EnemyWall.GetComponent<EnemyWallMovement> ().OffsetZ += 10f;
			LastObstaclePos = col.transform.position;
		}
	}
}
