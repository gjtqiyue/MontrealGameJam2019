using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public void Switch()
    {
        // call the function to switch the camera
        Debug.Log("Switch back the camera");
        this.GetComponentInParent<FPController>().SwitchCamera();
    }
}
