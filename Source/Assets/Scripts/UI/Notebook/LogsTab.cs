using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsTab : NotebookTab
{
    [SerializeField] private GameObject logPrefab;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(string log)
    {
        if (dataManager.CheckIfLogAlreadyExists(log))
            return;
        dataManager.LogsListInNotebook.Add(log);
        InstantiateLog(log);
    }

    public void InstantiateLog(string logData)
    {
        GameObject logObject = Instantiate(logPrefab, scrollViewContent.transform);
        logObject.GetComponent<LogObject>().InitializeLog(logData);
    }
    
}
