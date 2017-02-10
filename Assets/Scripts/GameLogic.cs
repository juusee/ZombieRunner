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

	static string[] Floor1;
	static string[] Floor0;
	static string[] Floor_1;
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

	float Floor(int floorNumber) {
		return PlatformSpawnPoint.position.y + floorNumber * FloorHeight;
	}

	void BuildFloors() {
		int currentPhase = PlatformCount % Phases;

		string floor1Thing = Floor1 [currentPhase];
		string floor0Thing = Floor0 [currentPhase];
		string floor_1Thing = Floor_1 [currentPhase];

		// Walls
		if (PlatformCount % 3 == 0) {
			SpawnWall (Floor(0));
			SpawnWall (Floor(1));
			SpawnWall (Floor(-1));
			SpawnWall (Floor(2));
			SpawnWall (Floor(-2));
			SpawnWall (Floor(3));
			SpawnWall (Floor(-3));
		}
		
		if (floor1Thing != "H") {
			SpawnPlatform (Floor(1));
		}
		if (floor0Thing != "H") {
			SpawnPlatform (Floor(0));
		}
		if (floor_1Thing != "H") {
			SpawnPlatform (Floor(-1));
		}
		SpawnPlatform (Floor(2));
		SpawnPlatform (Floor(-2));
		SpawnPlatform (Floor(3));
		SpawnPlatform (Floor(-3));

		// HoleTriggers
		if (floor1Thing == "H") {
			SpawnHoleTrigger (Floor(1));
		}
		if (floor0Thing == "H") {
			SpawnHoleTrigger (Floor(0));
		}
		if (floor_1Thing == "H") {
			SpawnHoleTrigger (Floor(-1));
		}

		// Spotlights and Crates
		if (floor1Thing == "L") {
			SpawnSpotlight (Floor(1));
			SpawnCrate (Floor(1));
		}
		if (floor0Thing == "L") {
			SpawnSpotlight (Floor(0));
			SpawnCrate (Floor(0));
		}
		if (floor_1Thing == "L") {
			SpawnSpotlight (Floor(-1));
			SpawnCrate (Floor(-1));
		}

		// Trees
		if (floor1Thing == "T") {
			SpawnTree (Floor (1));
		}
		if (floor0Thing == "T") {
			SpawnTree (Floor (0));
		}
		if (floor_1Thing == "T") {
			SpawnTree (Floor (-1));
		}

		PlatformSpawnPoint.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			PlatformSpawnPoint.transform.position.y,
			PlatformSpawnPoint.transform.position.z + PlatformLength// + PlatformGap
		);

		PlatformCount++;
	}

	void SpawnWall(float floor) {
		GameObject wall = PrefabManager.GetPrefab (Wall);
		wall.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x - PlatformWidth / 2,
			floor + FloorHeight / 2,
			PlatformSpawnPoint.transform.position.z
		);
		wall.SetActive (true);
	}

	void SpawnSpotlight(float floor) {
		GameObject spotlight = null;
		spotlight = PrefabManager.GetPrefab (Spotlight);
		spotlight.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floor + FloorHeight,
			PlatformSpawnPoint.transform.position.z
		);
		spotlight.SetActive (true);
	}

	void SpawnPlatform(float floor) {
		GameObject platform = null;
		platform = PrefabManager.GetPrefab (Platform);		
		platform.transform.parent = GameObject.FindGameObjectWithTag ("Platforms").transform;
		platform.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floor,
			PlatformSpawnPoint.transform.position.z
		);
		platform.SetActive (true);
		// TODO find out why all of the children are not active. Same on crate
		foreach (Transform child in platform.transform) {
			child.gameObject.SetActive (true);
		}
	}
	
	void SpawnHoleTrigger(float floor) {
		GameObject holeTrigger = PrefabManager.GetPrefab(HoleTrigger);
		holeTrigger.transform.position = new Vector3 (
			PlatformSpawnPoint.transform.position.x,
			floor,
			PlatformSpawnPoint.transform.position.z
		);
		holeTrigger.SetActive (true);
	}

	void SpawnTree(float floor) {
		if (PlatformSpawnPoint.transform.position.z > 80) {
			GameObject tree = PrefabManager.GetPrefab (Tree);
			tree.transform.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				floor + FloorHeight,
				PlatformSpawnPoint.transform.position.z
			);
			tree.SetActive (true);
		}
	}

	void SpawnCrate(float floor) {
		if (PlatformSpawnPoint.transform.position.z > 80) {
			GameObject crate = PrefabManager.GetPrefab (Crate);
			crate.transform.position = new Vector3 (
				PlatformSpawnPoint.transform.position.x,
				floor + PlatformHeight / 2 + Crate.GetComponent<Renderer> ().bounds.size.y / 2,
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

		//Floor1 =  new string[] {"-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "H", "-"};
		//Floor0 =  new string[] {"-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-"};
		//Floor_1 = new string[] {"-", "-", "-", "-", "-", "-", "H", "-", "-", "-", "-", "-", "-", "-", "-", "-", "", "-", "H", "-", "-", "-", "-", "-"};

		Phases = Floor1.Length;
	}
}
