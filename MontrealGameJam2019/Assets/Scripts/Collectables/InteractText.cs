using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    public GameObject target;

    private void Update()
    {
        if (target != null)
            transform.LookAt(target.transform);
    }
}
