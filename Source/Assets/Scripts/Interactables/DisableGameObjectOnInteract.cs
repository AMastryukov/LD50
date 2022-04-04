using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOnInteract : MonoBehaviour
{

    [SerializeField] private GameObject toDisable;

    private bool isGameObjectOn = true;
    
    private void Start()
    {
        isGameObjectOn = toDisable.activeSelf;
    }
    
    
    public void OnInteract()
    {
        if (isGameObjectOn)
        {
            toDisable.SetActive(false);
            isGameObjectOn = false;
        }
        else
        {
            toDisable.SetActive(true);
            isGameObjectOn = true;
        }
    }
    
}
