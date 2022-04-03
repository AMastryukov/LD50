using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectTab : NotebookTab
{
    public List<SuspectData> Suspects { get; private set; }

    [SerializeField] private GameObject prefab;

    public void Add(SuspectData suspectData)
    {
        Suspects.Add(suspectData);

        // Ignore existing suspects
        foreach (var existingData in Suspects)
        {
            if (existingData == suspectData)
            {
                return;
            }
        }

        Suspects.Add(suspectData);

        GameObject personnelObject = Instantiate(prefab, content.gameObject.transform);
        personnelObject.GetComponent<SuspectNotebookEntry>().Initialize(suspectData);
    }
}
