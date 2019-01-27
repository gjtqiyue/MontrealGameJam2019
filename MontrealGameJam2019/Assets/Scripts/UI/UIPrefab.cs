using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIPrefab
{
	public string name;
	
	[SerializeField]
	private GameObject prefab;

	public GameObject Prefab {
		get {
			return prefab;
		}
	}
}
