using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCutSceneScript : MonoBehaviour
{

    public GameObject player;
    public Cinemachine.CinemachineVirtualCamera vcam3;
    private Animator anim;

    private void Awake()
    {
        anim = vcam3.gameObject.GetComponent<Animator>();
    }

    // set the cam3's position relevant to the player
    public void InitializePosition()
    {
        Vector3 pos = player.transform.GetChild(0).position;
        Quaternion rot = player.transform.GetChild(0).rotation;
        vcam3.gameObject.transform.position = pos;
        vcam3.gameObject.transform.rotation = rot;
        Debug.Log(vcam3.gameObject.transform.position);
    }

    public void TriggerAnimation()
    {
        player.gameObject.GetComponent<Animator>().enabled = true;
        player.gameObject.GetComponent<Animator>().SetBool("EndGame", true);
    }
}
