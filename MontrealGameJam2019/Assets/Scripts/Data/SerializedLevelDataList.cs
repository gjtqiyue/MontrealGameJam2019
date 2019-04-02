using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[CreateAssetMenu(fileName = "statesData", menuName = "createStatesData", order = 1)]
#endif
public class SerializedLevelDataList : ScriptableObject
{
	public List<LevelData> LevelDatas;
}

