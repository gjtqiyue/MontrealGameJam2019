using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCutSceneScript : MonoBehaviour
{

    public GameObject player;
    public Cinemachine.CinemachineVirtualCamera vcam3;

    // set the cam3's position relevant to the player
    public void InitializePosition()
    {
        Vector3 left = -player.transform.right;
        Vector3 pos = player.transform.position + left * 10 + Vector3.up * 5;
        vcam3.transform.position = pos;
    }
}
