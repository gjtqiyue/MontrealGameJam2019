using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenEyeEffect : MonoBehaviour
{
    public void Activate()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponent<Animator>().Play("OpenEyes");
    }

    public void Deactivate()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }
}
