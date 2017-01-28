using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {

	public Transform Player;
	public GameObject Wall;

	Vector3 InitialPos;
	Vector3 WallPos;
	float WallWidth;

	// Use this for initialization
	void Start () {
		WallWidth = Wall.GetComponent<Renderer> ().bounds.size.z;
		GameObject wall = GameObject.FindGameObjectWithTag ("Wall");
		WallPos = wall.transform.position;
		InitialPos = wall.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (WallPos.z < Player.position.z) {
			SpawnWall();
		}
	}

	void SpawnWall() {
		GameObject wall = PrefabManager.GetPrefab (Wall);
		WallPos = new Vector3 (WallPos.x, Player.position.y, WallPos.z + WallWidth);
		wall.transform.position = WallPos;
		wall.SetActive (true);
	}

	public void Reset() {
		WallPos = new Vector3(InitialPos.x, InitialPos.y, InitialPos.z - WallWidth);
	}
}
