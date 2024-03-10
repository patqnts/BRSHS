using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageScript : MonoBehaviour
{
    private SegregateGame game;
    private void Start()
    {
        game = FindObjectOfType<SegregateGame>();
    }

    private void OnDisable()
    {
        game.InstantiateRandomGarbage();
    }
}
//test update