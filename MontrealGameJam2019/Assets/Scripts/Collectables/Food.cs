using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Collectable
{
    public int fillPerFood = 3;
    public AK.Wwise.Event FoodEvent;

    public override void GetCollected(CharacterScript player)
    {
        // call the method to fill hunger
        Debug.Log("Get memory");
        if (player != null)
        {
            // randomly decide if the food is infected
            int random = Random.Range(0, 3);
            int type;
            if (random == 1) type = 1;
            else type = 0;
            player.FillHunger(type, fillPerFood);
            FoodEvent.Post(gameObject);
        }
    }
}
