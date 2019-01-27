using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryScript : MonoBehaviour


{

    public AK.Wwise.Event Memory;

    // Start is called before the first frame update
    void Start()
    {
        Memory.Post(gameObject);    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
