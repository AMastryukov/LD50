using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsTab : NotebookTab
{
    public List<string> Logs { get; private set; }

    [SerializeField] private GameObject logPrefab;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    protected override void Awake()
    {
        base.Awake();
        Logs=new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(string log)
    {
        foreach (var logText in Logs )
        {
            if (logText == log)
            {
                return;
            }
        }
        Logs.Add(log); 
        GameObject logObject = Instantiate(logPrefab, scrollViewContent.transform);
        logObject.GetComponent<LogObject>().InitializeLog(log);
    }
    
}
