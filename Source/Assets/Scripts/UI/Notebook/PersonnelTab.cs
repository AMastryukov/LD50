using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonnelTab : NotebookTab
{
    public List<PersonnelData> Personnel { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(PersonnelData personnel)
    {
        Personnel.Add(personnel);
        // TODO: Mahad update canvas
    }
}
