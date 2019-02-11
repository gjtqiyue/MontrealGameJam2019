using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    //void Start()

    public void OnClick()
    {
        AkSoundEngine.PostEvent("UIConfirm", gameObject);
    }



    // Update is called once per frame
}
