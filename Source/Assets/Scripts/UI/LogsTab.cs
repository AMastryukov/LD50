using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogsTab : NotebookTab
{
    public List<string> Log { get; private set; }
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
        Log.Add(log);
        // TODO: Mahad update canvas
    }
}
