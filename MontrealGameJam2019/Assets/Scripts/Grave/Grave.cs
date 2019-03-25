using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    [SerializeField]
    bool locked = true;

    bool trig = false;

    public InteractText t;

	private void Start() {
		t.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update()
    {
        if (GameFlowManager.Instance.GetCurrentGameState() == GameState.FoundGrave)
        {
            locked = false;

            if (Input.GetButtonDown("XboxA") && trig)
            {
                // trigger the ending
                t.gameObject.SetActive(false);
                GameFlowManager.Instance.TriggerEndingCutScene();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!locked)
        {
            // activate the text
            if (other.tag == "Player")
            {
                t.gameObject.SetActive(true);
                trig = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!locked)
        {
            if (other.tag == "Player")
            {
                t.gameObject.SetActive(false);
                trig = false;
            }
        }
    }
}
