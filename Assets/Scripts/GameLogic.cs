using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Platform;
	public GameObject Player;
	public Transform PlatformSpawnPoint;
	public GameObject PlayArea;
	public GameObject HoleTrigger;
	public GameObject Crate;

	List<GameObject> Platforms = new List<GameObject> ();
	List<GameObject> HoleTriggers = new List<GameObject> ();
	List<GameObject> Crates = new List<GameObject> ();
	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformLength;
	float PlatformHeight;
	float FloorHeight = 13f;
	List<float> HolePositions = new List<float>();
	Vector3 PlatformSpawnPointInitialPos;

	// Use this for initialization
	void Start () {
		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		PlatformHeight = Platform.GetComponent<Renderer> ().bounds.size.y;
		foreach (GameObject platform in GameObject.FindGameObjectsWithTag ("Platform")) {
			Platforms.Add (platform);
		}
		PlatformSpawnPointInitialPos = new Vector3 (0, 0, -40);
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
			if (Random.value < 0.1) {
				SpawnCrate (PlatformHeight / 2 + Crate.GetComponent<Renderer>().bounds.size.y / 2);
			}
		} else {
			HolePositions [0] = PlatformSpawnPoint.position.z;
			SpawnHoleTrigger (-PlatformHeight);
		}
		if (PlatformSpawnPoint.position.z - HolePositions [1] < 80f) {
			SpawnPlatform (1 * FloorHeight);
			if (Random.value < 0.1) {
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

	void SpawnCrate(float height) {
		GameObject crate = GetCrate ();
		crate.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y + height,
			PlatformSpawnPoint.transform.position.z
		);
		crate.SetActive (true);
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

	GameObject GetCrate() {
		GameObject c = null;
		for (int i = 0; i < Crates.Count; ++i) {
			if (!Crates[i].activeSelf) {
				c = Crates [i];
			}
		}
		if (c == null) {
			c = (GameObject) Instantiate (Crate);
			Crates.Add (c);
		}
		c.SetActive (false);
		return c;
	}

	public void Reset() {
		foreach (GameObject g in Platforms) {
			Destroy (g);
		}
		Platforms = new List<GameObject> ();
		foreach (GameObject g in HoleTriggers) {
			Destroy (g);
		}
		HoleTriggers = new List<GameObject> ();
		foreach (GameObject g in Crates) {
			Destroy (g);
		}
		Crates = new List<GameObject> ();
		HolePositions = new List<float> ();
		HolePositions.Add (-20f);
		HolePositions.Add (-40f);
		PlatformSpawnPoint.transform.position = PlatformSpawnPointInitialPos;
	}
}
