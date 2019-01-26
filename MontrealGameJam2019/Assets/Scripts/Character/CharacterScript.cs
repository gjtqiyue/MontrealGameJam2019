using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    public Text display;
    public float hungerLimit;
    public float hungerRate = 0.1f;
    public float hunger;
    public float memoryLoseRate = 0.5f;
    public float memoryLoseAmt = 0.1f;
    public float memoryLastTime = 20;
    public Dictionary<int, Memory> memories;
    public Queue<int> memCollectionOrder;

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
    }

    // Update is called once per frame
    void Update()
    {
        display.text = "Memory number: " + memCollectionOrder.Peek() + "\n"
                     + "Time left: " + memories[memCollectionOrder.Peek()].GetLeftTime() + "\n"
                     + "Hunger: " + hunger + "\n" + Time.time;

        if (PlayerEffectEnabled)
            CheckMemory();
    }

    // potentially we can set a flag vector in game manager to check for status
    public void SetPlayerEffectActive(bool value) { PlayerEffectEnabled = value; }

    public void InitializeOnStart()
    {
        // add the memory to the queque and the map
        ReceiveMemory(0);

        StartCoroutine(DecreaseMemory());
        StartCoroutine(DecreaseHunger());
    }

    // check the top of the queue see if the memory is already lost
    public void CheckMemory()
    {
        if (memCollectionOrder == null) return;

        int num = memCollectionOrder.Peek();
        if (memories[num].IsLost())
        {
            LoseMemory();
        }
    }

    // when receive the memory, we add the memory to the map and update the queue
    public void ReceiveMemory(int num)
    {
        // if the number already exist it means that the player obtained this memory before
        if (memories.ContainsKey(num))
        {
            memories[num] = new Memory(memoryLastTime); 
        }
        else
        {
            memories.Add(num, new Memory(memoryLastTime));
        }
        Debug.Log("receive memory " + num);
        memories[memCollectionOrder.Peek()].Recover();
        memCollectionOrder.Enqueue(num);
    }

    // get rid of one memory completely
    public void LoseMemory()
    {
        if (memCollectionOrder.Count > 0)
        {
            int num = memCollectionOrder.Dequeue();
            memories[num] = null;
        }
        else
        {
            Debug.Log("No more memory to lose");
        }
    }

    IEnumerator DecreaseMemory()
    {
        while (Time.time < 360)
        {
            // if the player is controlling the game
            if (PlayerEffectEnabled)
            {
                int num = memCollectionOrder.Peek();
                memories[num].Lose(memoryLoseAmt);
                yield return new WaitForSeconds(memoryLoseRate);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        Debug.Log("end coroutine");
    }

    // Hunger part
    IEnumerator DecreaseHunger()
    {
        while (Time.time < 360)
        {
            if (PlayerEffectEnabled)
            {
                hunger -= hungerRate;
                yield return new WaitForSeconds(2);
            }
            else
            {
                yield return new WaitForEndOfFrame();
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
                Debug.Log("ok food");
                break;
            case 1:
                // food is infected
                hunger += fill;
                Debug.Log("bad food");
                LoseMemory();
                break;
            default:
                break;
        }
    }
}
