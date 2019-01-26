using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : ManagerBase<GameFlowManager>
{
	public enum GameState {
		StartMenu,
		InGame,
		BeatGame,
		Dead
	}

	[SerializeField]
	private SerializeLevelDataList levelDatas;

	[SerializeField]
	private GameObject player;

	[SerializeField]
	private Camera mainCamera;

	private GameState gameState = GameState.StartMenu;

	private LevelData currentLevel;

	public event Action<LevelData> OnLevelRefresh;
	public event Action<LevelData> OnPlayerDead;
	public event Action<LevelData> OnWin;

	public bool StartGame() {
		if (gameState != GameState.StartMenu) return false;
		player.SetActive(true);
		mainCamera.gameObject.SetActive(false);
		currentLevel = levelDatas.LevelDatas[0];
		gameState = GameState.InGame;
		UIManager.Instance.StartGame();
		return true;
	}

	public bool DecreaseLevel() {
		if (gameState != GameState.InGame) return false;
		if (currentLevel.id == 0) return PlayerDead();
		ChangeLevel(currentLevel.id - 1);

		return true;
	}

	public bool IncreaseLevel() {
		if (gameState != GameState.InGame) return false;
		if (currentLevel.id >= levelDatas.LevelDatas.Count) return Win();

		ChangeLevel(currentLevel.id + 1);
		return true;
	}

	public bool PlayerDead() {
		if (gameState != GameState.InGame) return false;
		gameState = GameState.Dead;
		UIManager.Instance.DeadGame();
		safeInvoke(OnPlayerDead);
		return true;
	}

	private void ChangeLevel(int level) {
		if(level < 0 || level >= levelDatas.LevelDatas.Count) {
			Debug.LogError("Calling unreachable Level");
		} else {
			currentLevel = levelDatas.LevelDatas[currentLevel.id - 1];
			safeInvoke(OnLevelRefresh);
		}
	}

	private bool Win() {
		if (gameState != GameState.InGame) return false;

		gameState = GameState.BeatGame;
		safeInvoke(OnWin);
		return true;
	}

	private void safeInvoke(Action<LevelData> a) {
		if (a != null) {
			a.Invoke(currentLevel);
		}
	}
}
