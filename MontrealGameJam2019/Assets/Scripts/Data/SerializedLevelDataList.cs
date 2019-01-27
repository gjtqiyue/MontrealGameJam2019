using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "statesData", menuName = "createStatesData", order = 1)]
[System.Serializable]
public class SerializeLevelDataList : ScriptableObject
{
	public List<LevelData> LevelDatas;
}
