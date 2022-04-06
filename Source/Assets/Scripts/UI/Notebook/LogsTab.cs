using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsTab : NotebookTab
{
    [SerializeField] private GameObject logPrefab;

    public void Add(string log)
    {
        if (DataManager.Instance.CheckIfLogAlreadyExists(log))
            return;
        DataManager.Instance.NotebookLog.Add(log);
        InstantiateLog(log);
    }

    public void InstantiateLog(string logData)
    {
        GameObject logObject = Instantiate(logPrefab, scrollViewContent.transform);
        logObject.GetComponent<LogNotebookEntry>().InitializeLog(logData);
    }
    
}
