using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum GameState
{
    StartMenu,
    InGame,
    FoundGrave,
    BeatGame,
    Dead
}

public class GameFlowManager : ManagerBase<GameFlowManager>
{

	[SerializeField]
	private SerializeLevelDataList levelDatas;

	[SerializeField]
	private GameObject player;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private PlayableDirector titleTimeline;

    [SerializeField]
    private PlayableDirector endingTimeline;

    private GameState gameState = GameState.StartMenu;

	private LevelData currentLevel;

	private InGameUI inGameUI;

    private Narrative storyTeller;

	public event Action<LevelData> OnLevelRefresh;
	public event Action<LevelData> OnPlayerDead;
	public event Action<LevelData> OnWin;

	public void Start() {
        endingTimeline.Stop();
		titleTimeline.Stop();
        storyTeller = UIManager.Instance.gameObject.GetComponentInChildren<Narrative>();
    }

	public IEnumerator StartGame() {
		if (gameState != GameState.StartMenu) yield break;
		titleTimeline.Play();

		yield return new WaitForSeconds(2.5f);

        // trigger the open eyes effect
        OpenEyeEffect eye = UIManager.Instance.gameObject.GetComponentInChildren<OpenEyeEffect>();
        if (eye != null) eye.Activate();
        else Debug.Log("No eye");

		player.SetActive(true);

        inGameUI = UIManager.Instance.StartGame().GetComponent<InGameUI>();

        mainCamera.gameObject.SetActive(false);
		titleTimeline.time = 0;
		currentLevel = levelDatas.LevelDatas[0];
		gameState = GameState.InGame;

        storyTeller.OnNarrativeSpeak("Where am I...... \n and....\n who am I.......");

        // trigger the cuffin to open the door
        player.GetComponent<enableWakeup>().OpenCoffin();

        yield return null;
	}

    public IEnumerator AquireTheFirstMemory()
    {
        // Look at the note and think about it
        // TODO

        yield return new WaitForSeconds(2);

        storyTeller.OnNarrativeSpeak("What home means to me...... maybe I will figure it out if I can find more clue");

        CharacterScript sc = player.GetComponent<CharacterScript>();
 
        if (inGameUI != null)
        {
            inGameUI.Initialize(sc);
        }

        ActivateInGameUI();

        // enable the movement control and game starts
        player.GetComponent<CharacterScript>().enabled = true;
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<FPController>().PlayerMovementEnabled = true;

        yield return null;
    }

	//activate the in game ui
	private void ActivateInGameUI() {
		if(inGameUI != null) {
			inGameUI.ActivateUI();
		}
	}

	public bool DecreaseLevel() {
		if (gameState != GameState.InGame) return false;
		if (currentLevel.id == 0) return PlayerDead();
		ChangeLevel(currentLevel.id - 1);

		return true;
	}

	public bool IncreaseLevel() {
		if (gameState != GameState.InGame) return false;
		if (currentLevel.id >= levelDatas.LevelDatas.Count) return FoundGrave();

		ChangeLevel(currentLevel.id + 1);
		return true;
	}

	public bool PlayerDead() {
		if (gameState != GameState.InGame) return false;
		gameState = GameState.Dead;
		CharacterScript sc = player.GetComponent<CharacterScript>();
		if (inGameUI != null) {
			inGameUI.Destroy(sc);
		}
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

        OpenEyeEffect eye = UIManager.Instance.gameObject.GetComponentInChildren<OpenEyeEffect>();
        if (eye != null) eye.Close();

        gameState = GameState.BeatGame;
		safeInvoke(OnWin);
		return true;
	}

	private void safeInvoke(Action<LevelData> a) {
		if (a != null) {
			a.Invoke(currentLevel);
		}
	}

    private bool FoundGrave()
    {
        gameState = GameState.FoundGrave;

        storyTeller.OnNarrativeSpeak("Now I remember...\n my family, \n myself, and ... \n my home.");

        return true;
    }

    public void HungerWarning()
    {
        storyTeller.HungerWarning();
    }

    public void TriggerEndingCutScene()
    {
        endingTimeline.GetComponent<EndGameCutSceneScript>().InitializePosition();

        // disable the player camera
        player.transform.GetChild(0).GetComponent<Camera>().enabled = false;

        // play the time line
        endingTimeline.Play();

        // set the sun to turn
        // TODO: fine the directional light and trigger the daynightswitch

        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(10);

        Win();
    }

    public GameState GetCurrentGameState()
    {
        return gameState;
    }
}
