using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

	Transform PlatformSpawnPoint;

	// Use this for initialization
	void Start () {
		PlatformSpawnPoint = GameObject.FindGameObjectWithTag ("PlatformSpawnPoint").transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
