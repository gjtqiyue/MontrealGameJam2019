using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWakeup : MonoBehaviour
{
  public Animator coffinDoorAnim;
  public Animator malcolmAnim;
  public Animator characterAnim;
    public AK.Wwise.Event GraveOpen;

    private void Start()
    {
        characterAnim.SetBool("Intro", true);
    }

    // Update is called once per frame
    void Update()
    {
      if (coffinDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RotateDoor"))
        characterAnim.SetTrigger("EnableWakeup");

    }

    public void ExitIntro()
    {
        characterAnim.SetBool("Intro", false);
        characterAnim.enabled = false;

        // call the game manager to show the player the note
        GameFlowManager.Instance.StartCoroutine(GameFlowManager.Instance.AquireTheFirstMemory());
    }

    public void OpenCoffin()
    {
        coffinDoorAnim.SetTrigger("CoffinDoorOpen");
        GraveOpen.Post(gameObject);
    }
}
