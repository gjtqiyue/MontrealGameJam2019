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

    public void Close()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponent<Animator>().Play("CloseEyes");
    }

    public void ShowText()
    {
        transform.GetChild(0).GetComponent<Text>().enabled = true;
        transform.GetChild(0).GetComponent<Text>().text = "The home is where the family is";
    }
}
