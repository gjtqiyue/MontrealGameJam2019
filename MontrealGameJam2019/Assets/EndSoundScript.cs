using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSoundScript : MonoBehaviour
{
    public void OnClick()
    {
        AkSoundEngine.PostEvent("UIEND", gameObject); 
    }

}
