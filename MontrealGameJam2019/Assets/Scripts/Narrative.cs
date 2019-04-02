using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrative : MonoBehaviour
{
    Animator anim;
    Text t;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        t = gameObject.GetComponent<Text>();
    }

    public IEnumerator OnNarrativeSpeak(string sentence)
    {
        yield return new WaitForSeconds(1.5f);
        t.text = sentence;
        anim.Play("FadeIn");
    }

    public void HungerWarning()
    {
        StartCoroutine(WarningText());
    }

    IEnumerator WarningText()
    {
        t.text = "I feel hungury...... I need something to eat";
        anim.Play("FadeIn");
        yield return new WaitForSeconds(10);
    }
}
