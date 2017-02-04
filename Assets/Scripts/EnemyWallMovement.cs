using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallMovement : MonoBehaviour {

	public Transform Player;
	public float OffsetZ;

	float InitialOffsetZ;
	float OffsetX;
	float OffsetY;
	float SmoothTime = 0.1f;
	Vector3 Velocity = Vector3.zero;
	Vector3 StartPosition;
	float AmountToIncreaseGap = 0.01f;

	void Awake () {
		OffsetX = transform.position.x - Player.position.x;
		OffsetY = transform.position.y - Player.position.y;
		InitialOffsetZ = OffsetZ = transform.position.z - Player.position.z;
		StartPosition = transform.position;
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.z - Player.position.z > InitialOffsetZ) {
			OffsetZ -= AmountToIncreaseGap;
		}
		Vector3 targetPosition = new Vector3 (
			Player.position.x + OffsetX,
			Player.position.y + OffsetY,
			Player.position.z + OffsetZ
		);
		// Needs to have interpolation on on target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref Velocity, SmoothTime);
	}

	public void Reset() {
		transform.position = StartPosition;
	}
}
