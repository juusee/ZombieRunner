using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Platform;
	public GameObject PlatformWithLight;
	public GameObject Player;
	public GameObject HoleTrigger;
	public GameObject Crate;
	public GameObject PlayArea;
	public Transform PlatformSpawnPoint;

	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformLength;
	float PlatformHeight;
	float FloorHeight = 13f;
	Vector3 PlatformSpawnPointInitialPos;

	float PlatformCount = 0;

	void Awake() {
		PlatformSpawnPointInitialPos = PlatformSpawnPoint.transform.position;
	}

	// Use this for initialization
	void Start () {
		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		PlatformHeight = Platform.GetComponent<Renderer> ().bounds.size.y;
		foreach (GameObject platform in GameObject.FindGameObjectsWithTag ("Platform")) {
			PrefabManager.AddPrefab (Platform, platform);
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
		PlatformCount++;
		bool light2 = PlatformCount % 10 == 0;
		bool light1 = PlatformCount % 5 == 0 && !light2;
		if ((PlatformCount + 7) % 12 != 0 || PlatformSpawnPoint.position.z < 80) {
			SpawnPlatform (0, light1);
			if (light2) {
				SpawnCrate (PlatformHeight / 2 + Crate.GetComponent<Renderer> ().bounds.size.y / 2);
			}
		} else {
			SpawnHoleTrigger (-PlatformHeight);
		}
		if ((PlatformCount + 3) % 12 != 0 || PlatformSpawnPoint.position.z < 80) {
			SpawnPlatform (1 * FloorHeight, light2);
			if (light1) {
				SpawnCrate (FloorHeight + PlatformHeight / 2 + Crate.GetComponent<Renderer> ().bounds.size.y / 2);
			}
		} else {
			SpawnHoleTrigger (FloorHeight - PlatformHeight);
		}
		if ((PlatformCount - 1) % 12 != 0 || PlatformSpawnPoint.position.z < 80) {
			SpawnPlatform (-1 * FloorHeight, light2);
			if (light1) {
				SpawnCrate (-FloorHeight + PlatformHeight / 2 + Crate.GetComponent<Renderer> ().bounds.size.y / 2);
			}
		} else {
			SpawnHoleTrigger (-FloorHeight - PlatformHeight);
		}
		SpawnPlatform (2 * FloorHeight, light1);
		SpawnPlatform (-2 * FloorHeight, light1);
		SpawnPlatform (3 * FloorHeight, light2);
		SpawnPlatform (-3 * FloorHeight, light2);
		PlatformSpawnPoint.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y,
			PlatformSpawnPoint.transform.position.z + PlatformLength// + PlatformGap
		);
	}

	void SpawnPlatform(float height, bool light) {
		GameObject platform = null;
		if (light) {
			platform = PrefabManager.GetPrefab (PlatformWithLight);
		} else {
			platform = PrefabManager.GetPrefab (Platform);		
		}
		platform.transform.parent = GameObject.FindGameObjectWithTag ("Platforms").transform;
		platform.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z
		);
		platform.SetActive (true);
		// TODO find out why all of the children are not active. Same on crate
		foreach (Transform child in platform.transform) {
			child.gameObject.SetActive (true);
		}
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

	void SpawnCrate(float height) {
		if (PlatformSpawnPoint.transform.position.z > 80) {
			GameObject crate = PrefabManager.GetPrefab (Crate);
			crate.transform.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				PlatformSpawnPoint.transform.position.y + height,
				PlatformSpawnPoint.transform.position.z
			);
			crate.SetActive (true);
			crate.transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	public void Reset() {
		PlatformCount = 0;
		PlatformSpawnPoint.transform.position = PlatformSpawnPointInitialPos;
	}
}
