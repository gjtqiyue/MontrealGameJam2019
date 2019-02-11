using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAmbiance : MonoBehaviour
{
    public AK.Wwise.Event MenuAmbiant;

    // Start is called before the first frame update
    void Start()
    {
        MenuAmbiant.Post(gameObject);   
    }

    // Update is called once per frame
   
}
