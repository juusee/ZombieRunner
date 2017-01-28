using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	static Dictionary<GameObject, List<GameObject>> PrefabLists = new Dictionary<GameObject, List<GameObject>>();

	public static void AddPrefab(GameObject prefab, GameObject g) {
		if (!PrefabLists.ContainsKey (prefab)) {
			PrefabLists.Add (prefab, new List<GameObject> ());
		}
		PrefabLists [prefab].Add (g);
	}

	public static GameObject GetPrefab(GameObject prefab) {
		GameObject g = null;
		if (!PrefabLists.ContainsKey (prefab)) {
			PrefabLists.Add (prefab, new List<GameObject> ());
		} else {
			foreach (GameObject p in PrefabLists[prefab]) {
				if (!p.activeSelf) {
					g = p;
					break;
				}
			}
		}
		if (g == null) {
			g = (GameObject) Instantiate (prefab);
			PrefabLists[prefab].Add (g);
		}
		return g;
	}

	public static void Reset() {
		var buffer = new List<GameObject>(PrefabLists.Keys);
		foreach (GameObject key in buffer) {
			for (int i = 0; i < PrefabLists [key].Count; ++i) {
				Destroy (PrefabLists [key][i]);
			}
			PrefabLists [key] = new List<GameObject> ();
		}
	}
}
