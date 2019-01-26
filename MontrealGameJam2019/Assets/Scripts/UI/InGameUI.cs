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


	private void Awake() {
		memories = new Queue<Image>();
		AddMemory(3);
		//StartCoroutine(decreaseMemory());
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
		Image memory = memories.Peek();
		if(memory.color.a > amount) {
			memory.color = new Color(memory.color.r, memory.color.g, memory.color.b, memory.color.a-amount);
		} else {
			memories.Dequeue();
			Destroy(memory.gameObject);
		}
	}

	private IEnumerator decreaseMemory() {
		while (true) {
			ChangeHunger(-0.1f);
			yield return new WaitForSeconds(0.2f);
		}
		
	}

	private void ChangeHunger(float amount) {
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
		memories.Enqueue(image);
	}

	private void FillLastMemory(float fillAmount) {
		var memory = memories.Peek();
		memory.color = new Color(memory.color.r, memory.color.g, memory.color.b, memory.color.a + fillAmount);

	}
}
