using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
	[SerializeField]
	public string name;

	[SerializeField]
	public int id;

	[SerializeField]
	public int numberOfFood;

	[SerializeField]
	public float totalTimer;

	[SerializeField]
	public float hunger;
}
