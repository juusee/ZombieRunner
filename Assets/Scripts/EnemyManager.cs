using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	GameObject[] Enemies;

	void Start() {
		Enemies = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public void Reset() {
		foreach (GameObject enemy in Enemies) {
			enemy.SetActive (false);
			enemy.SetActive (true);
		}
	}

	public void DisableControl() {
		foreach (GameObject enemy in Enemies) {
			enemy.GetComponent<EnemyMovement>().enabled = false;
			enemy.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	public void EnableControl() {
		foreach (GameObject enemy in Enemies) {
			enemy.GetComponent<EnemyMovement>().enabled = true;
			enemy.GetComponent<Rigidbody> ().isKinematic = false;
		}
	}
}
