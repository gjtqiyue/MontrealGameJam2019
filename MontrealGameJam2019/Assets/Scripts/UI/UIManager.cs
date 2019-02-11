using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase<UIManager>
{
	[SerializeField]
	private List<UIPrefab> uiPrefabs;

	[SerializeField]
	private Transform uiParent;

	private Dictionary<string, GameObject> activeUIs;

	//names to relate the ui and prefab
	private string titleUIName = "title";
	private string inGameUIName = "inGame";
	private string deadGameUIName = "dead";
    private string endGaameUIName = "end";

	public void Start() {
		activeUIs = new Dictionary<string, GameObject>();
		AddUI(titleUIName);
	}

	public void DeadGame() {
		RemoveAndAdd(inGameUIName, deadGameUIName);

	}

	public GameObject StartGame() {
		RemoveAndAdd(titleUIName, inGameUIName);
		return activeUIs[inGameUIName];
	}

	private void RemoveAndAdd(string uiToRemove, string uiToAdd) {
		GameObject ui = activeUIs[uiToRemove];
		if(ui != null) {
			activeUIs.Remove(uiToRemove);
			Destroy(ui);
		}
		
		AddUI(uiToAdd);
	}

	private void AddUI(string name) {
		UIPrefab prefab = uiPrefabs.Find((ui) => ui.name.Equals(name));
		if (prefab != null) {
			GameObject title = Instantiate(prefab.Prefab, uiParent);
			activeUIs.Add(prefab.name, title);
		}
	}

    public void EndGame()
    {
        RemoveAndAdd(inGameUIName, endGaameUIName);
    }
}
