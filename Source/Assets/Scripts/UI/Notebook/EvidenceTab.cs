using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    public List<EvidenceData> EvidenceList { get; private set; }

    [SerializeField] private GameObject evidencePrefab;
    
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
        GameObject evidenceObject = Instantiate(evidencePrefab, content.gameObject.transform);
        evidenceObject.GetComponent<EvidenceNotebookEntry>().InitializeEvidence(evidence);
    }
}
