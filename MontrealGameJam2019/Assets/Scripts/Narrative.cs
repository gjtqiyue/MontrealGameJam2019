using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrative : MonoBehaviour
{
    public Animator storyanim;
    public Animator warninganim;
    public Text storyT;
    public Text warningT;

    float timer = -1;
    int curLine;

    string[] memoryLines = new string[]
        {
            "There's a dirty photo on the ground, the figures are barely recognizeable, however it feels nostalgic looking at it.",
            "The woman's smiling face pulls at my heart. Who is she?",
            "Next to the woman, a kind looking man looks back at me....he looks....fatherly....\n.......dad...?\n a glipse of that man appeared in my mind, saying \"It's okay baby....it'll be alright\" to the voice of a crying child",
            "A smug face appeared next to the couple. He must be my brother...his face in my memory is blurry \n but I remember he always protected me from the bullies. ",
            "Warmth spread through my body as I saw the couple standing together.  Memories came flooding in like a rainstorm...\n Our first meeting, the night he said \"I love you\" and our the day we got married. \n Everything came back to me."
        };


    private void Update()
    {
        // set a timer to trigger the middle brother line
        if (curLine == 2)
        {
            curLine = -1;
            timer = 0;
        }

        if (timer != -1)
        {
            timer += Time.deltaTime;
        }

        if (timer > 15)
        {
            RecallBrotherInTheMiddle();
            timer = -1;
        }
    }

    public void WakeUpLine()
    {
        StartCoroutine(OnNarrativeSpeak("\"What happened?...\n......Where am I...... \n ....\n ........... \n who am I.......\"\nI woke up to darkness and couldn't remember anything", 2f));
    }

    public void RecallFamilyMembers(int idx)
    {
        curLine = idx;
        StartCoroutine(OnNarrativeSpeak(memoryLines[idx], 1.5f));
    }

    public void RecallAllFamily(int time)
    {
        StartCoroutine(OnNarrativeSpeak("I remember.  I remember everything!\n My parents, my brother, the love of my life....\n ....Where are they...?", time));

    }

    private IEnumerator OnNarrativeSpeak(string sentence, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        storyT.text = sentence;
        storyanim.Play("FadeIn");
    }

    public void RecallBrotherInTheMiddle()
    {
        StartCoroutine(OnNarrativeSpeak("My father would often comfort my brother and I after we hurt ourselves playing.", 1.5f));
    }

    public void HungerWarning()
    {
        StartCoroutine(WarningText("I feel hungry...... I need something to eat"));
    }

    public void MonsterWarning()
    {
        StartCoroutine(WarningText("I heard noises, something is coming towards me...."));
    }

    IEnumerator WarningText(string text)
    {
        warningT.text = text;
        warninganim.Play("FadeIn");
        yield return new WaitForSeconds(5);
    }
}
