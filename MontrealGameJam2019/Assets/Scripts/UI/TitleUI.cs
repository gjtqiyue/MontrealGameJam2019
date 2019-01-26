using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
	public void OnStartClicked() {
		GameFlowManager.Instance.StartGame();
	}

	public void OnEndClicked() {
		Application.Quit();
	}
}
