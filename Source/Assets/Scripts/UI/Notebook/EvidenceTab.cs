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
        dataManager.NotebookEvidence.Add(name);
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
        AudioManager.Instance.OnNoteBookScribble();
        PlayerInteractor.OnEvidenceFoundNotification?.Invoke(GameConstants.HudConstants.EvidenceNotification);
        GameObject evidenceObject = Instantiate(evidencePrefab, content.gameObject.transform);
        evidenceObject.GetComponent<EvidenceNotebookEntry>().InitializeEvidence(evidence);
    }
}
