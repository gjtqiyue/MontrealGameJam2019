using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWakeup : MonoBehaviour
{
  public Animator coffinDoorAnim;
  public Animator malcolmAnim;
  public Animator characterAnim;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      if (coffinDoorAnim.GetCurrentAnimatorStateInfo(0).IsName("RotateDoor"))
      characterAnim.SetTrigger("EnableWakeup");

    }
}
