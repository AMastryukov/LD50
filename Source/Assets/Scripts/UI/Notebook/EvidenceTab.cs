using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    [SerializeField] private GameObject evidencePrefab;
    
    public void Add(EvidenceData evidence)
    {
        if(dataManager.CheckIfEvidenceAlreadyExists(evidence))
            return;
        
        dataManager.NotebookEvidence.Add(evidence);
        InstantiateEvidence(evidence);
    }

    public void InstantiateEvidence(EvidenceData evidence)
    {
        if (evidence == null)
        {
            Debug.LogError("Data not found");
            return;
        }
        GameObject evidenceObject = Instantiate(evidencePrefab, scrollViewContent.gameObject.transform);
        evidenceObject.GetComponent<EvidenceNotebookEntry>().InitializeEvidence(evidence);
    }
}
