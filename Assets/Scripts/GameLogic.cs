using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	public GameObject Wall;
	public GameObject Platform;
	public GameObject Spotlight;
	public GameObject Player;
	public GameObject HoleTrigger;
	public GameObject Crate;
	public GameObject Tree;
	public GameObject PlayArea;
	public Transform PlatformSpawnPoint;

	float PlatformSpawnLengthFromPlayer = 60f;
	float PlatformLength;
	float PlatformHeight;
	float PlatformWidth;
	float FloorHeight = 13f;
	Vector3 PlatformSpawnPointInitialPos;

	int PlatformCount;
	float InitialFloorHeight;
	int FloorCount = 7;

	static string[] Floor1;
	static string[] Floor0;
	static string[] Floor_1;
	static string[][] Floors;
	static int Phases;

	void Awake() {
		PlatformSpawnPointInitialPos = PlatformSpawnPoint.transform.position;
	}

	// Use this for initialization
	void Start () {
		PlatformWidth = Platform.GetComponent<Renderer> ().bounds.size.x;
		PlatformHeight = Platform.GetComponent<Renderer> ().bounds.size.y;
		PlatformLength = Platform.GetComponent<Renderer> ().bounds.size.z;
		foreach (GameObject platform in GameObject.FindGameObjectsWithTag ("Platform")) {
			PrefabManager.AddPrefab (Platform, platform);
		}

		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		// TODO maybe make use PlayArea
		if (PlatformSpawnPoint.position.z - Player.transform.position.z < PlatformSpawnLengthFromPlayer) {
			BuildFloors ();
		}

		// DEVELOPMENT
		/*if (Input.GetKeyDown (KeyCode.Alpha1))
			SetFloors (-1);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			SetFloors (0);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			SetFloors (1);*/
	}

	float FloorPositionY(int floorNumber) {
		// TODO maybe improve Floors.Length * FloorHeight
		return PlatformSpawnPoint.position.y + floorNumber * FloorHeight - FloorCount / 2 * FloorHeight;
	}

	void BuildFloor(int floor) {
		int currentPhase = PlatformCount % Phases;

		// TODO better var name and improve
		int floorShape = (int) Mathf.Abs((Mathf.Abs(floor) + PlayerMovement.CurrentFloor / FloorHeight)) % Floors.Length;
		string floorThing = Floors[floorShape][currentPhase];

		// Wall
		if (PlatformCount % 3 == 0) {
			SpawnWall (FloorPositionY(floor));
		}

		// TODO
		if (FloorCount % 2 == 0) {
			Debug.LogError ("WRONG FLOOR COUNT");
		}

		// Floor else hole
		if (floorThing != "H") {
			SpawnPlatform (FloorPositionY (floor), floorShape);
		}
		// HoleTrigger
		if (floorThing == "H") {
			SpawnHoleTrigger (FloorPositionY (floor));
		}
		// FloorThings only on current floor or one above or below
		if (floor == FloorCount / 2 || floor == FloorCount / 2 + 1 || floor == FloorCount / 2 - 1) {
			// Spotlight and Crate
			if (floorThing == "L") {
				SpawnSpotlight (FloorPositionY (floor));
				SpawnCrate (FloorPositionY (floor));
			}

			// Tree
			if (floorThing == "T") {
				SpawnTree (FloorPositionY (floor));
			}
		}
	}

	void BuildFloors() {

		// TODO Maybe improve
		for (int i = 0; i < FloorCount; ++i) {
			BuildFloor (i);
		}

		PlatformSpawnPoint.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y,
			PlatformSpawnPoint.transform.position.z + PlatformLength// + PlatformGap
		);

		PlatformCount++;
	}

	void SpawnWall(float floorPositionY) {
		GameObject wall = PrefabManager.GetPrefab (Wall);
		wall.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x - PlatformWidth / 2,
			floorPositionY + FloorHeight / 2,
			PlatformSpawnPoint.transform.position.z
		);
		wall.SetActive (true);
	}

	void SpawnSpotlight(float floorPositionY) {
		GameObject spotlight = null;
		spotlight = PrefabManager.GetPrefab (Spotlight);
		spotlight.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floorPositionY + FloorHeight,
			PlatformSpawnPoint.transform.position.z
		);
		spotlight.SetActive (true);
	}

	void SpawnPlatform(float floorPositionY, int jou) {
		GameObject platform = null;
		platform = PrefabManager.GetPrefab (Platform);		
		platform.transform.parent = GameObject.FindGameObjectWithTag ("Platforms").transform;
		platform.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floorPositionY,
			PlatformSpawnPoint.transform.position.z
		);
		// DEBUG
		if (jou == 0)
			platform.GetComponent<Renderer> ().material.color = Color.blue;
		if (jou == 1)
			platform.GetComponent<Renderer> ().material.color = Color.red;
		if (jou == 2)
			platform.GetComponent<Renderer> ().material.color = Color.green;
		
		platform.SetActive (true);
		// TODO find out why all of the children are not active. Same on crate
		foreach (Transform child in platform.transform) {
			child.gameObject.SetActive (true);
		}
	}
	
	void SpawnHoleTrigger(float floorPositionY) {
		GameObject holeTrigger = PrefabManager.GetPrefab(HoleTrigger);
		holeTrigger.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floorPositionY,
			PlatformSpawnPoint.transform.position.z
		);
		holeTrigger.SetActive (true);
	}

	void SpawnTree(float floorPositionY) {
		if (PlatformSpawnPoint.transform.position.z > 80) {
			GameObject tree = PrefabManager.GetPrefab (Tree);
			tree.transform.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				floorPositionY + FloorHeight,
				PlatformSpawnPoint.transform.position.z
			);
			tree.SetActive (true);
			tree.transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	void SpawnCrate(float floorPositionY) {
		if (PlatformSpawnPoint.transform.position.z > 80) {
			GameObject crate = PrefabManager.GetPrefab (Crate);
			crate.transform.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				floorPositionY + PlatformHeight / 2 + Crate.GetComponent<Renderer> ().bounds.size.y / 2,
				PlatformSpawnPoint.transform.position.z
			);
			crate.SetActive (true);
			crate.transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	// DEVELOPMENT
	/*public static void SetFloors(int jou) {
		Floor0 =  new string[24];
		Floor1 =  new string[24];
		Floor_1 =  new string[24];
		for (int i = 0; i < Floor0.Length; ++i) {
			Floor0[i] = "-";
			Floor1[i] = "-";
			Floor_1[i] = "-";
			if (jou == 1 && i % 14 == 0) {
				Floor1[i] = "H";
			}
			if (jou == 0 && i % 10 == 0) {
				Floor0[i] = "H";
			}
			if (jou == -1 && i % 18 == 0) {
				Floor_1[i] = "H";
			}
		}
		Phases = Floor1.Length;
	}*/

	public void Reset() {
		PlatformCount = 1;
		PlatformSpawnPoint.transform.position = PlatformSpawnPointInitialPos;
		Floor1 =  new string[] {"L", "-", "-", "H", "-", "-", "-", "-", "L", "-", "T", "-", "-", "-", "H", "-", "L", "-", "-", "-", "-", "-", "H", "-"};
		Floor0 =  new string[] {"-", "-", "-", "-", "L", "-", "-", "-", "-", "-", "H", "-", "L", "-", "-", "-", "-", "T", "-", "-", "L", "-", "-", "-"};
		Floor_1 = new string[] {"L", "-", "T", "-", "-", "-", "H", "-", "L", "-", "-", "-", "-", "-", "-", "-", "L", "-", "H", "-", "-", "-", "-", "-"};

		Floors = new string[][] { Floor_1, Floor0, Floor1 };

		//Floor1 =  new string[] {"-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "H", "-"};
		//Floor0 =  new string[] {"-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-"};
		//Floor_1 = new string[] {"-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "", "-", "H", "-", "-", "-", "-", "-"};

		Phases = Floor1.Length;
	}
}
