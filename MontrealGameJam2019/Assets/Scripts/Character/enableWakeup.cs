﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWakeup : MonoBehaviour
{
    public AK.Wwise.Event CoffinAudio;
  public Animator coffinDoorAnim;
  public Animator malcolmAnim;
  public Animator characterAnim;

    // Update is called once per frame
    void Update()
    {
      if (coffinDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RotateDoor"))
        characterAnim.SetTrigger("EnableWakeup");

    }

    public void DeactivateAnimator()
    {
        characterAnim.enabled = false;

        // call the game manager to show the player the note
        GameFlowManager.Instance.StartCoroutine(GameFlowManager.Instance.AquireTheFirstMemory());
    }

    public void OpenCoffin()
    {
        coffinDoorAnim.SetTrigger("CoffinDoorOpen");
        CoffinAudio.Post(gameObject);
    }
}
