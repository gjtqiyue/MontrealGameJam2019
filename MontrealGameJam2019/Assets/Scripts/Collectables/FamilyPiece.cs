using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamilyPiece : Collectable
{
    public AK.Wwise.Event Memory;
    public int pieceNum;
    public override void GetCollected(CharacterScript player)
    {
        // call the method to add memory
        Debug.Log("Get memory");
        if (player != null) player.ReceiveMemory(pieceNum);
        Memory.Post(gameObject);
             
             }
}

