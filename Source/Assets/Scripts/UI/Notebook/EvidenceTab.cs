using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    public List<EvidenceData> EvidenceList { get; private set; }

    [SerializeField] private GameObject evidencePrefab;
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
        foreach (var evidenceData in EvidenceList )
        {
            if (evidenceData == evidence)
            {
                return;
            }
        }
        EvidenceList.Add(evidence);
        GameObject logObject = Instantiate(evidencePrefab, content.gameObject.transform);
        logObject.GetComponent<EvidenceObject>().InitializeEvidence(evidence);
    }
}
