using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationBench : MonoBehaviour
{

    private PlayerManager manager;
    
    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
    }

    public void OnBenchInteract()
    {
        manager.playerState = PlayerManager.PlayerStates.Interrogate;
    }
    
}
