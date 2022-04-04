using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Non-persistant game scene manager that fires events telling the persistent game manager to change state
/// </summary>
public class CrimeSceneManager : MonoBehaviour
{
    public static Action OnAllEvidenceFound;

    private List<EvidenceData> evidenceData;
    private Notebook notebook;

    void Awake()
    {
        #region Find and Filter all Required Evidence
        evidenceData = new List<EvidenceData>();
        var allEvidence = FindObjectsOfType<Evidence>();

        foreach(var evidenceItem in allEvidence)
        {
            evidenceData.Add(evidenceItem.evidenceData);
        }

        // Keep only unique evidence datas
        evidenceData = evidenceData.Distinct().ToList();
        #endregion

        notebook = FindObjectOfType<Notebook>();

        Evidence.OnInspect += EvidenceFound;
    }

    private void OnDestroy()
    {
        Evidence.OnInspect -= EvidenceFound;
    }

    private void EvidenceFound(Evidence evidence)
    {
        StartCoroutine(CheckForAllEvidenceFound());
    }

    private IEnumerator CheckForAllEvidenceFound()
    {
        // Wait a frame for evidence to be added to notebook
        yield return null;

        // Check the player's notebook
        foreach (EvidenceData evidence in evidenceData)
        {
            if (!DataManager.Instance.NotebookEvidence.Contains(evidence.Name)) { yield break; }
        }

        print("OnAllEvidenceFound");
        OnAllEvidenceFound?.Invoke();
    }

    // DEV-ONLY: Automatically finds all evidence in the scene
    public void FindAllEvidenceDebug()
    {
        foreach (var evidence in evidenceData)
        {
            notebook?.AddEvidence(evidence.Name);
        }

        OnAllEvidenceFound?.Invoke();

        Debug.Log("[DEBUG] All evidence added");
    }
}
