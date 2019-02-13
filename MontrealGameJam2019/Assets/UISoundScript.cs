using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void onClick()
    {
        AkSoundEngine.PostEvent("UIConfirm", gameObject);
    }
}
