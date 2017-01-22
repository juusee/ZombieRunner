using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Platform;
	public GameObject Player;
	public Transform PlatformSpawnPoint;
	public GameObject PlayArea;

	List<GameObject> Platforms = new List<GameObject> ();
	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformGap = 15;
	float PlatformLength;
	List<float> HolePositions = new List<float>();

	// Use this for initialization
	void Start () {
		HolePositions.Add (-20f);
		HolePositions.Add (-40f);

		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		foreach (GameObject platform in GameObject.FindGameObjectsWithTag ("Platform")) {
			Platforms.Add (platform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// TODO maybe make use PlayArea
		if (PlatformSpawnPoint.position.z - Player.transform.position.z < PlatformSpawnLengthFromPlayer) {
			BuildFloors ();
		}
	}

	void BuildFloors() {
		float floorHeight = 13f;
		if (PlatformSpawnPoint.position.z - HolePositions [0] < 80f) {
			SpawnPlatform (0);
		} else {
			HolePositions [0] = PlatformSpawnPoint.position.z;
		}
		if (PlatformSpawnPoint.position.z - HolePositions [1] < 80f) {
			SpawnPlatform (1 * floorHeight);
		} else {
			HolePositions [1] = PlatformSpawnPoint.position.z;
		}
		SpawnPlatform (-1 * floorHeight);
		SpawnPlatform (2 * floorHeight);
		SpawnPlatform (-2 * floorHeight);
		SpawnPlatform (3 * floorHeight);
		SpawnPlatform (-3 * floorHeight);
		SpawnPlatform (4 * floorHeight);
		SpawnPlatform (-4 * floorHeight);
		PlatformSpawnPoint.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y,
			PlatformSpawnPoint.transform.position.z + PlatformLength// + PlatformGap
		);
	}

	void SpawnPlatform(float height) {
		GameObject platform = GetPlaform ();
		if (height == 0) {
			//platform.transform.localScale = new Ve
		}
		platform.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z
		);
		platform.SetActive (true);
	}

	GameObject GetPlaform() {
		GameObject p = null;
		for (int i = 0; i < Platforms.Count; ++i) {
			if (!Platforms[i].activeSelf) {
				p = Platforms [i];
			}
		}
		if (p == null) {
			p = (GameObject) Instantiate (Platform);
			Platforms.Add (p);
			p.name = "Platform_" + (Platforms.Count - 1).ToString ();
		}
		p.SetActive (false);
		return p;
	}
}
