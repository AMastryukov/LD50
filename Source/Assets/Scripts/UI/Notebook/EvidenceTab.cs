using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    [SerializeField] private GameObject evidencePrefab;
    
    public void Add(EvidenceKey key)
    {
        if(dataManager.CheckIfEvidenceAlreadyExists(key))
            return;
        dataManager.evidenceListInNotebook.Add(key);
        EvidenceData data = dataManager.GetEvidenceDataFromKey(key);
        InstantiateEvidence(data);
    }

    public void InstantiateEvidence(EvidenceData evidence)
    {
        if (evidence == null)
            return;
        GameObject evidenceObject = Instantiate(evidencePrefab, content.gameObject.transform);
        evidenceObject.GetComponent<EvidenceObject>().InitializeEvidence(evidence);
    }
}
