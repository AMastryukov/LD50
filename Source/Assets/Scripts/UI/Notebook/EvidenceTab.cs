using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    [SerializeField] private GameObject evidencePrefab;
    
    public void Add(string name)
    {
        if(dataManager.CheckIfEvidenceAlreadyExists(name))
            return;
        
        dataManager.evidenceListInNotebook.Add(name);
        EvidenceData data = dataManager.GetEvidenceDataFromKey(name);
        InstantiateEvidence(data);
    }

    public void InstantiateEvidence(EvidenceData evidence)
    {
        if (evidence == null)
        {
            Debug.LogError("Data not found");
            return;
        }
        GameObject evidenceObject = Instantiate(evidencePrefab, content.gameObject.transform);
        evidenceObject.GetComponent<EvidenceObject>().InitializeEvidence(evidence);
    }
}
