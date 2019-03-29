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
    public bool testButton = false;

	[SerializeField]
	private SerializeLevelDataList levelDatas;

    [SerializeField]
    private AutoIntensity nightDaySwitchScript;

	[SerializeField]
	private GameObject player;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private PlayableDirector titleTimeline;

    [SerializeField]
    private PlayableDirector endingTimeline;
    
    [SerializeField]
    private GameState gameState = GameState.StartMenu;

    [SerializeField]
	private LevelData currentLevel;

	private InGameUI inGameUI;

    private Narrative storyTeller;

	public event Action<LevelData> OnLevelRefresh;
	public event Action<LevelData> OnPlayerDead;
	public event Action<LevelData> OnWin;

	public void Start() {
        endingTimeline.enabled = false;
		titleTimeline.Stop();
    }

    private void Update()
    {
        if (testButton)
        {
            TriggerEndingCutScene();
            testButton = false;
        }
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
        titleTimeline.enabled = false;

		gameState = GameState.InGame;

        storyTeller = UIManager.Instance.gameObject.GetComponentInChildren<Narrative>();

        // trigger the cuffin to open the door
        player.GetComponent<enableWakeup>().OpenCoffin();

        StartCoroutine(storyTeller.OnNarrativeSpeak("\"Where am I...... \n and....\n who am I.......\"\nYou woke up but you realize you can't remember anything"));

        yield return null;
	}

    public IEnumerator AquireTheFirstMemory()
    {
        // Look at the note and think about it
        // TODO

        yield return new WaitForSeconds(2);

        StartCoroutine(storyTeller.OnNarrativeSpeak("You found a dirty photo on the ground which is too blury to recognize, maybe you will figure it out if you can find more clue"));

        yield return new WaitForSeconds(1);

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
		if (currentLevel.id+1 >= levelDatas.LevelDatas.Count-1) return FoundGrave();

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
        Debug.Log("change to " + level);
		if(level < 0 || level > levelDatas.LevelDatas.Count) {
			Debug.LogError("Calling unreachable Level");
		} else {
			currentLevel = levelDatas.LevelDatas[level];
			safeInvoke(OnLevelRefresh);
		}
	}

	private bool Win() {
		if (gameState != GameState.FoundGrave) return false;

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
        Debug.Log("found grave");
        OnLevelRefresh -= FamilyPieceManager.Instance.SpawnNewMemory;

        ChangeLevel(currentLevel.id + 1);

        StartCoroutine(storyTeller.OnNarrativeSpeak("Now you remember...\n about your family, \n about yourself, and ... \n about your home\n You decide to go find the final place of your family"));

        return true;
    }

    public void HungerWarning()
    {
        if (gameState == GameState.InGame) storyTeller.HungerWarning();
    }

    public void TriggerEndingCutScene()
    {
        player.gameObject.SetActive(false);
        endingTimeline.enabled = true;
        endingTimeline.GetComponent<EndGameCutSceneScript>().InitializePosition();
        endingTimeline.GetComponent<EndGameCutSceneScript>().TriggerAnimation();

        // disable the player camera
        player.transform.GetChild(0).GetComponent<Camera>().enabled = false;

        titleTimeline.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);

        endingTimeline.time = 0;

        // disable all the UI element
        UIManager.Instance.EndGame();

        // play the time line
        endingTimeline.Play();
        Debug.Log("play");

        // set the sun to turn
        // find the directional light and trigger the daynightswitch
        nightDaySwitchScript.nightDaySwitch = true;

        StartCoroutine(WaitForEnd());
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(15);

        Win();
    }

    public GameState GetCurrentGameState()
    {
        return gameState;
    }
}
