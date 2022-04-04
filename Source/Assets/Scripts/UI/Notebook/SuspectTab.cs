using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectTab : NotebookTab
{
    [SerializeField] private GameObject prefab;

    public void Add(SuspectData suspect)
    {
        if(dataManager.CheckIfSuspectAlreadyExists(suspect))
            return;

        dataManager.NotebookSuspects.Add(suspect);
        InstantiateSuspect(suspect);
    }
    
    public void InstantiateSuspect(SuspectData suspect)
    {
        if (suspect == null)
        {
            Debug.LogError("Data not found");
            return;
        }

        GameObject evidenceObject = Instantiate(prefab, scrollViewContent.transform);
        evidenceObject.GetComponent<SuspectNotebookEntry>().Initialize(suspect);
    }
}
