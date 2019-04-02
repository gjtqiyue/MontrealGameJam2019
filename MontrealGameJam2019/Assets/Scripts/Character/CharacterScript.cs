using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public GameObject flashLight;
    public Text display;
    public float hungerLimit;
    public float hungerRate = 0.1f;
	[Header("Initialize to 100 pls")]
    public float hunger;
    public float memoryLoseRate = 0.005f;
    public float memoryLoseAmt = 5;
    public float memoryLastTime = 20;
    public Dictionary<int, Memory> memories;
    public Queue<int> memCollectionOrder;
	public event Action<float> OnHungerChanged;
	public event Action<float> OnMemoryIncreased;
	public event Action<float> OnMemoryDecreased;

    [SerializeField]
    private int damage;

	private FPController fpController;
    [SerializeField]
    private bool PlayerEffectEnabled;              // the effect will work when player is controlling

    // Start is called before the first frame update
    void Start()
    {
        memories = new Dictionary<int, Memory>();
        memCollectionOrder = new Queue<int>();
        fpController = GetComponent<FPController>();
        InitializeOnStart();
		GameFlowManager.Instance.OnLevelRefresh += refreshHunger;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerEffectEnabled)
        {
            CheckMemory();
            CheckHunger();
        }

        // game over
        if (hunger <= 0 || memCollectionOrder.Count == 0)
        {
			GameFlowManager.Instance.PlayerDead();
        }
    }

    // potentially we can set a flag vector in game manager to check for status
    public void SetPlayerEffectActive(bool value) { PlayerEffectEnabled = value; }

    public void InitializeOnStart()
    {
        // enable flashlight
        flashLight.SetActive(true);

        // add the memory to the queque and the map
        ReceiveMemory(0);

		// start with half hunger
		hunger -= hungerLimit / 2;
		if(OnHungerChanged != null) {
			OnHungerChanged.Invoke(-0.5f);
		}

		StartCoroutine(DecreaseMemory());
        StartCoroutine(DecreaseHunger());
    }

    // check the top of the queue see if the memory is already lost
    public void CheckMemory()
    {
        if (memCollectionOrder == null || memCollectionOrder.Count <= 0) return;

        int num = memCollectionOrder.Peek();
        if (memories[num] != null && memories[num].IsLost())
        {
			LoseMemory();
        }
    }

    public void CheckHunger()
    {
        if (hunger == 15)
        {
            // warn the player to find some food
            GameFlowManager.Instance.HungerWarning();
        }
    }

    // when receive the memory, we add the memory to the map and update the queue
    public void ReceiveMemory(int num)
    {
        StartCoroutine(GameFlowManager.Instance.AquireMemory(num));
		
        // if the number already exist it means that the player obtained this memory before
        if (memories.ContainsKey(num))
        {
            memories[num] = new Memory(memoryLastTime); 
        }
        else
        {
            memories.Add(num, new Memory(memoryLastTime));
        }

		if(OnMemoryDecreased != null) {
			OnMemoryIncreased(1);
		}

		Debug.Log("receive memory " + num);
        if(memCollectionOrder.Count > 0) memories[memCollectionOrder.Peek()].Recover();

        memCollectionOrder.Enqueue(num);

        // show the photo to the player
        Debug.Log("show photo of " + num);
       
	    StartCoroutine(ShowPhoto(num));
    }

	IEnumerator ShowPhoto(int num) {
		GameObject obj = FamilyPieceManager.Instance.GetPhotoPiece(num);
		Transform mainCam = transform.GetChild(0);
        yield return new WaitForSeconds(5);
        GameObject photo = Instantiate(obj, mainCam.position + (transform.forward * 1), Quaternion.identity, transform);
		photo.transform.localRotation = Quaternion.Euler(90,180,0);
		yield return new WaitForSeconds(5);
		Destroy(photo.gameObject);
	}

    // get rid of one memory completely
    public void LoseMemory()
    {
        if (memCollectionOrder.Count > 0)
        {
            int num = memCollectionOrder.Dequeue();
            memories[num] = null;

			OnMemoryDecreased.Invoke(1);
		} else
        {
            GameFlowManager.Instance.PlayerDead();
        }
    }

    IEnumerator DecreaseMemory()
    {
        while (Time.time < 5000 && memCollectionOrder.Count > 0)
        {
            // if the player is controlling the game
            if (PlayerEffectEnabled && memCollectionOrder.Count>0)
            {
				if(memCollectionOrder.Count > 0) {
					int num = memCollectionOrder.Peek();
					memories[num].Lose(memoryLoseAmt);
					if(OnMemoryDecreased != null) {
						OnMemoryDecreased.Invoke(memoryLoseAmt / memories[num].GetMaxTime());
					}
					yield return new WaitForSeconds(memoryLoseRate);
				} else {
					yield break;
				}
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        Debug.Log("end coroutine");
    }

	public void refreshHunger(LevelData data) {
		if(data.hunger > 100) {
			return;
		}
		float hungerChange = data.hunger - hunger;
		hunger = data.hunger;
		OnHungerChanged.Invoke(hungerChange/hungerLimit);
	}

    // Hunger part
    IEnumerator DecreaseHunger()
    {
        while (Time.time < 5000 && hunger > 0)
        {
            if (PlayerEffectEnabled)
            {
                hunger -= hungerRate;
				if(OnHungerChanged != null) {
					OnHungerChanged.Invoke(-hungerRate / hungerLimit);
				}
				yield return new WaitForSeconds(2);
            }
            else
            {
                yield return null;
            }
        }
        Debug.Log("end coroutine");
    }

    public void FillHunger(int type, int fill)
    {
        switch (type) {
            case 0:
                // food is ok
                hunger += fill;
				OnHungerChanged.Invoke(fill / hungerLimit);
                Debug.Log("ok food");
                break;
            case 1:
                // food is infected
                hunger += fill;
				OnHungerChanged.Invoke(fill / hungerLimit);
				Debug.Log("bad food");
                LoseMemory();
                break;
            default:
                break;
        }
    }

    public void TriggerPickUpAnimation()
    {
        fpController.PickUp();
    }

    public void GetHitByMonster()
    {
        damage++;
        if (damage > 3)
        {
            damage = 0;
            LoseMemory();
        }
    }


}
