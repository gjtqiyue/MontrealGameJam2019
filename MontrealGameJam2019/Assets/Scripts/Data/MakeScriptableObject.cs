using UnityEngine;
using System.Collections;
using UnityEditor;


public class MakeScriptableObject {
#if UNITY_EDITOR
	[MenuItem("Assets/Create/State Data List")]
	public static void CreateMyAsset() {
		SerializedLevelDataList asset = ScriptableObject.CreateInstance<SerializedLevelDataList>();

		AssetDatabase.CreateAsset(asset, "Assets/SerializedStateDataList.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
#endif
	}
}