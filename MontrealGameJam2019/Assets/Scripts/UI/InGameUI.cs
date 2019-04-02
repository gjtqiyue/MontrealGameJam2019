using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
	private GameObject UIObject;
	[SerializeField]
	private GameObject memoryPrefab;
	[SerializeField]
	private Queue<Image> memories;
	[SerializeField]
	private GameObject memoryParent;
	[SerializeField]
	private float memoryDistance;

	[SerializeField]
	private Image hungerIndicator;

    [SerializeField]
    private Text feedbackText;

    [SerializeField]
	private GameObject minimap;

	[SerializeField]
	private GameObject hungerPrefab;

	public bool IsUIShown = false;
	private void Awake() {
		memories = new Queue<Image>();
	}

	//initialize to add ui action when the player states changed
	public void Initialize(CharacterScript c) {
		c.OnHungerChanged += ChangeHunger;
		c.OnMemoryDecreased += DecreaseMemory;
		c.OnMemoryIncreased += AddMemory;
	}

	//initialize to add ui action when the player states changed
	public void Destroy(CharacterScript c) {
		c.OnHungerChanged -= ChangeHunger;
		c.OnMemoryDecreased -= DecreaseMemory;
		c.OnMemoryIncreased -= AddMemory;
	}

	public void ActivateUI() {
		hungerPrefab.SetActive(true);
		minimap.SetActive(true);
		foreach(Image i in memories) {
			i.gameObject.SetActive(true);
		}
		IsUIShown = true;
	}

	public void AddMemory(float memory) {
		while (memory > 0) {
			float thisFill = 0;
			if (memories.Count == 0 || memories.Peek().fillAmount >= 1) {
				thisFill = memory >= 1 ? 1 : memory;
				CreateFullMemory(thisFill);
			} else {
				float amountNeeded = 1 - memories.Peek().fillAmount;
				thisFill = memory >= amountNeeded ? amountNeeded : memory;
				FillLastMemory(thisFill);
			}
			memory -= thisFill;
		}
    }

	public void DecreaseMemory(float amount) {
		if (memories.Count <= 0) {
			Debug.Log(11);
			return;

		}

		Image memory = memories.Peek();
		if (memory == null) return;


		if(memory.color.a > amount) {
			memory.color = new Color(255, 255, 255, memory.color.a-amount);
		} else {
			memories.Dequeue();
			Destroy(memory.gameObject);

		}
	}

	private void ChangeHunger(float amount) {
		if (hungerIndicator == null) return;

		if(hungerIndicator.fillAmount + amount > 1) {
			hungerIndicator.fillAmount = 1;
		}else if(hungerIndicator.fillAmount + amount < 0) {
			hungerIndicator.fillAmount = 0;
		} else {
			hungerIndicator.fillAmount += amount;
		}
	}

	private void CreateFullMemory(float fillAmount) {
		var memory = Instantiate(memoryPrefab, memoryParent.transform);
		memory.transform.localPosition = new Vector3(memoryDistance * memories.Count, 0, 0);
		var image = memory.GetComponent<Image>();
		image.fillAmount = fillAmount;
		if (!IsUIShown) {
			image.gameObject.SetActive(false);
		}
		memories.Enqueue(image);
	}

	private void FillLastMemory(float fillAmount) {
		var memory = memories.Peek();
		memory.color = new Color(memory.color.r, memory.color.g, memory.color.b, memory.color.a + fillAmount);

	}
}
