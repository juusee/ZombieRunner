using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public PlayerManager PlayerManager;
	public EnemyManager EnemyManager;
	public WallManager WallManager;
	public GameLogic GameLogic;
	public GameObject Menu;
	public EnemyWallMovement EnemyWall;

	float endDelay = 2.5f;
	WaitForSeconds endWait;
	static bool start = false;

	void Start () {
		endWait = new WaitForSeconds (endDelay);
		StartCoroutine (GameLoop());
	}

	IEnumerator GameLoop () {
		PlayerManager.DisableControl ();
		PlayerManager.Reset ();
		EnemyManager.DisableControl ();
		EnemyManager.Reset ();
		GameLogic.Reset ();
		WallManager.Reset ();
		PrefabManager.Reset ();
		EnemyWall.Reset ();
		Menu.SetActive (true);

		yield return StartCoroutine (RoundStarting());

		Menu.SetActive (false);

		yield return StartCoroutine (RoundPlaying());
		yield return StartCoroutine (RoundEnding());
		StartCoroutine (GameLoop ());
	}

	IEnumerator RoundStarting () {
		while (!start) {
			yield return null;
		}
		start = false;
	}

	IEnumerator RoundPlaying () {
		PlayerManager.EnableControl ();
		EnemyManager.EnableControl ();
		while (PlayerManager.PlayerInstance.activeSelf) {
			yield return null;
		}
	}

	IEnumerator RoundEnding () {
		PlayerManager.DisableControl ();
		yield return endWait;
	}

	public void StartGame() {
		start = true;	
	}
}
