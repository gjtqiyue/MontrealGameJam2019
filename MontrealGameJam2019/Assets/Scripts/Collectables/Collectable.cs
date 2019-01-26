﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public float textDist = 15;

    private CharacterScript player;
    private InteractText text;
    private float dist;
    private bool trig = false;

    protected void Start()
    {    
        text = transform.GetChild(0).gameObject.GetComponent<InteractText>();
        text.gameObject.SetActive(false);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            text.gameObject.SetActive(true);
            player = other.gameObject.GetComponent<CharacterScript>();
            trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            text.gameObject.SetActive(false);
            player = null;
            trig = false;
        }
    }


    protected void Update()
    {
        if (Input.GetButtonDown("XboxA") && trig)
        {
            Debug.Log("pick item");
            GetCollected(player);
            this.gameObject.SetActive(false);
        }
    }

    // it will be overrided by different type of object
    public abstract void GetCollected(CharacterScript player);
}