using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectTab : NotebookTab
{
    [SerializeField] private GameObject prefab;

    public void Add(string suspectName)
    {
        if(dataManager.CheckIfSuspectAlreadyExists(suspectName))
            return;

        dataManager.NotebookSuspects.Add(suspectName);
        SuspectData suspectData = dataManager.GetSuspectDataFromKey(suspectName);
        InstantiateSuspect(suspectData);
    }
    
    public void InstantiateSuspect(SuspectData suspect)
    {
        if (suspect == null)
        {
            Debug.LogError("Data not found");
            return;
        }

        GameObject evidenceObject = Instantiate(prefab, content.gameObject.transform);
        evidenceObject.GetComponent<SuspectNotebookEntry>().Initialize(suspect);
    }
}
