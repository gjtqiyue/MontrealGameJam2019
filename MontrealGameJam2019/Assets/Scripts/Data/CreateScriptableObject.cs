using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject {
	[MenuItem("Assets/Create/State Data List")]
	public static void CreateMyAsset() {
		SerializeLevelDataList asset = ScriptableObject.CreateInstance<SerializeLevelDataList>();

		AssetDatabase.CreateAsset(asset, "Assets/SerializedStateDataList.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}