using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Platform;
	//public GameObject PlatformWithLight;
	public GameObject Player;
	public GameObject HoleTrigger;
	public GameObject Crate;
	public GameObject PlayArea;
	public Transform PlatformSpawnPoint;

	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformLength;
	float PlatformHeight;
	float FloorHeight = 13f;
	List<float> HolePositions = new List<float>();
	List<float> PlatformsWithLightsPositions = new List<float>();
	Vector3 PlatformSpawnPointInitialPos;

	// Use this for initialization
	void Start () {
		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		PlatformHeight = Platform.GetComponent<Renderer> ().bounds.size.y;
		foreach (GameObject platform in GameObject.FindGameObjectsWithTag ("Platform")) {
			PrefabManager.AddToListOfPrefabs (Platform, platform);
		}
		PlatformSpawnPointInitialPos = new Vector3 (0, 0, -40);

		PrefabManager.SetListOfPrefabs (Platform);
		//PrefabManager.SetListOfPrefabs (PlatformWithLight);
		PrefabManager.SetListOfPrefabs (HoleTrigger);
		PrefabManager.SetListOfPrefabs (Crate);
	}
	
	// Update is called once per frame
	void Update () {
		// TODO maybe make use PlayArea
		if (PlatformSpawnPoint.position.z - Player.transform.position.z < PlatformSpawnLengthFromPlayer) {
			BuildFloors ();
		}
	}

	void BuildFloors() {		
		if (PlatformSpawnPoint.position.z - HolePositions [0] < 80f) {
			SpawnPlatform (0);
			if (Random.value < 0.1 && Player.transform.position.z > 80f) {
				SpawnCrate (PlatformHeight / 2 + Crate.GetComponent<Renderer>().bounds.size.y / 2);
			}
		} else {
			HolePositions [0] = PlatformSpawnPoint.position.z;
			SpawnHoleTrigger (-PlatformHeight);
		}
		if (PlatformSpawnPoint.position.z - HolePositions [1] < 80f) {
			SpawnPlatform (1 * FloorHeight);
			if (Random.value < 0.1 && Player.transform.position.z > 80f) {
				SpawnCrate (PlatformHeight / 2 + Crate.GetComponent<Renderer>().bounds.size.y / 2);
			}
		} else {
			HolePositions [1] = PlatformSpawnPoint.position.z;
			SpawnHoleTrigger (FloorHeight - PlatformHeight);
		}
		SpawnPlatform (-1 * FloorHeight);
		SpawnPlatform (2 * FloorHeight);
		SpawnPlatform (-2 * FloorHeight);
		SpawnPlatform (3 * FloorHeight);
		SpawnPlatform (-3 * FloorHeight);
		SpawnPlatform (4 * FloorHeight);
		SpawnPlatform (-4 * FloorHeight);
		PlatformSpawnPoint.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y,
			PlatformSpawnPoint.transform.position.z + PlatformLength// + PlatformGap
		);
	}

	void SpawnHoleTrigger(float height) {
		GameObject holeTrigger = PrefabManager.GetPrefab(HoleTrigger);
		holeTrigger.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			Player.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z - PlatformLength / 2
		);
		holeTrigger.SetActive (true);
	}

	void SpawnPlatform(float height) {
		GameObject platform = null;
		//if (PlatformSpawnPoint.position.z - PlatformsWithLightsPositions [0] < 40f) {
			platform = PrefabManager.GetPrefab (Platform);		
		//} else {
		//	platform = PrefabManager.GetPrefab (PlatformWithLight);
		//	PlatformsWithLightsPositions [0] = PlatformSpawnPoint.position.z;
		//}
		platform.transform.parent = GameObject.FindGameObjectWithTag ("Platforms").transform;
		platform.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z
		);
		platform.SetActive (true);
	}

	void SpawnCrate(float height) {
		GameObject crate = PrefabManager.GetPrefab (Crate);
		crate.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z
		);
		crate.SetActive (true);
	}

	public void Reset() {
		PlatformsWithLightsPositions = new List<float> ();
		PlatformsWithLightsPositions.Add (0f);
		HolePositions = new List<float> ();
		HolePositions.Add (-20f);
		HolePositions.Add (-40f);
		PlatformSpawnPoint.transform.position = PlatformSpawnPointInitialPos;
	}
}
