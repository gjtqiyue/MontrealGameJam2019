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

    public void OnNarrativeSpeak(string sentence)
    { 
        StopCoroutine(Show(sentence));
        StartCoroutine(Show(sentence));
    }

    IEnumerator Show(string sentence)
    {
        yield return new WaitForSeconds(2);
        t.text = sentence;
        anim.Play("FadeIn");
    }
}
