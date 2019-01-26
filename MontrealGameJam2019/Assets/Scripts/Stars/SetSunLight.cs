using UnityEngine;
using System.Collections;

public class SetSunLight : MonoBehaviour {

    Material sky;

    public Transform stars;

    // Use this for initialization
    void Start ()
    {

        sky = RenderSettings.skybox;

    }

    bool lighton = false;

    // Update is called once per frame
    void Update ()
    {

        stars.transform.rotation = transform.rotation;

    }
}
