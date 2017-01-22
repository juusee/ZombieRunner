using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Platform;
	public GameObject Player;
	public Transform PlatformSpawnPoint;
	public GameObject PlayArea;
	public GameObject HoleTrigger;

	List<GameObject> Platforms = new List<GameObject> ();
	List<GameObject> HoleTriggers = new List<GameObject> ();
	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformGap = 15;
	float PlatformLength;
	float PlatformHeight;
	float FloorHeight = 13f;
	List<float> HolePositions = new List<float>();

	// Use this for initialization
	void Start () {
		HolePositions.Add (-20f);
		HolePositions.Add (-40f);

		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		PlatformHeight = Platform.GetComponent<Renderer> ().bounds.size.y;
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
		if (PlatformSpawnPoint.position.z - HolePositions [0] < 80f) {
			SpawnPlatform (0);
		} else {
			HolePositions [0] = PlatformSpawnPoint.position.z;
			SpawnHoleTrigger (-PlatformHeight);
		}
		if (PlatformSpawnPoint.position.z - HolePositions [1] < 80f) {
			SpawnPlatform (1 * FloorHeight);
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
		GameObject holeTrigger = GetHoleTrigger ();
		holeTrigger.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			Player.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z - PlatformLength / 2
		);
		holeTrigger.SetActive (true);
	}

	void SpawnPlatform(float height) {
		GameObject platform = GetPlaform ();
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
			p.transform.parent = GameObject.FindGameObjectWithTag ("Platforms").transform;
		}
		p.SetActive (false);
		return p;
	}

	GameObject GetHoleTrigger() {
		GameObject h = null;
		for (int i = 0; i < HoleTriggers.Count; ++i) {
			if (!HoleTriggers[i].activeSelf) {
				h = HoleTriggers [i];
			}
		}
		if (h == null) {
			h = (GameObject) Instantiate (HoleTrigger);
			HoleTriggers.Add (h);
		}
		h.SetActive (false);
		return h;
	}
}
