using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTab : NotebookTab
{
    [SerializeField] private GameObject evidencePrefab;
    
    public void Add(EvidenceData evidence)
    {
        if (DataManager.Instance.CheckIfEvidenceAlreadyExists(evidence))
            return;

        DataManager.Instance.NotebookEvidence.Add(evidence);
        InstantiateEvidence(evidence);
    }

    public void InstantiateEvidence(EvidenceData evidence)
    {
        if (evidence == null)
        {
            Debug.LogError("Data not found");
            return;
        }
        
        AudioManager.Instance.PlayNotebookScribble();
        PlayerInteractor.OnEvidenceFoundNotification?.Invoke(GameConstants.HudConstants.EvidenceNotification);
        GameObject evidenceObject = Instantiate(evidencePrefab, scrollViewContent.transform);
        evidenceObject.GetComponent<EvidenceNotebookEntry>().InitializeEvidence(evidence);
    }

    public void ClearEvidence()
    {
        foreach(Transform item in scrollViewContent.transform)
        {
            Destroy(item.gameObject);
        }
    }
}
