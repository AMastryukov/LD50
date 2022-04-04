using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsTab : NotebookTab
{
    [SerializeField] private GameObject logPrefab;

    public void Add(string log)
    {
        if (dataManager.CheckIfLogAlreadyExists(log))
            return;
        dataManager.NotebookLog.Add(log);
        InstantiateLog(log);
    }

    public void InstantiateLog(string logData)
    {
        GameObject logObject = Instantiate(logPrefab, scrollViewContent.transform);
        logObject.GetComponent<LogNotebookEntry>().InitializeLog(logData);
    }
    
}
