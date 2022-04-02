using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{

    public List<EvidenceData> Evidence { get; private set; }

    public List<PersonnelData> Personnel { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(EvidenceData evidence)
    {
        Evidence.Add(evidence);
    }

    public void Add(PersonnelData personnel)
    {
        Personnel.Add(personnel);
    }




}
