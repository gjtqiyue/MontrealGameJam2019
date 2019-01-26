using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
	public void OnStartClicked() {
		StartCoroutine(GameFlowManager.Instance.StartGame());
	}

	public void OnEndClicked() {
		Application.Quit();
	}
}
