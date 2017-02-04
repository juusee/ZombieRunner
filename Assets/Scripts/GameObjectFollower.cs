using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFollower : MonoBehaviour {

	public Transform GameObjectToFollow;
	public bool FollowX;
	public bool FollowY;
	public bool FollowZ;

	float OffsetZ;
	float OffsetX;
	float OffsetY;
	float SmoothTime = 0.1f;
	Vector3 Velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		OffsetX = transform.position.x - GameObjectToFollow.position.x;
		OffsetY = transform.position.y - GameObjectToFollow.position.y;
		OffsetZ = transform.position.z - GameObjectToFollow.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPosition = new Vector3 (
			FollowX ? GameObjectToFollow.position.x + OffsetX : transform.position.x,
			FollowY ? GameObjectToFollow.position.y + OffsetY : transform.position.y,
			FollowZ ? GameObjectToFollow.position.z + OffsetZ : transform.position.z
		);
		// Needs to have interpolation on on target
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref Velocity, SmoothTime);
	}
}
