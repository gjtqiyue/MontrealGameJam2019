using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAmbiance : MonoBehaviour
{
    public AK.Wwise.Event AudioStart;

    // Start is called before the first frame update
    void Start()
    {
        AudioStart.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
