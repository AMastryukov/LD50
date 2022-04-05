using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;
using TMPro;

/// <summary>
/// Non-persistant game scene manager that fires events telling the persistent game manager to change state
/// </summary>
public class CrimeSceneManager : MonoBehaviour
{
    public static Action OnAllEvidenceFound;

    private List<EvidenceData> evidenceData;
    private Notebook notebook;
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Canvas progressUI;
    [SerializeField] private Slider progressSlider;

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
        PlayerManager.OnPlayerStateChanged += UpdateCanvas;

        progressSlider.value = 0;

        objectiveText.text = $"Discover all Evidence ({0}/{evidenceData.Count})";
    }

    private void OnDestroy()
    {
        Evidence.OnInspect -= EvidenceFound;
    }

    private void EvidenceFound(Evidence evidence)
    {
        objectiveText.text = $"Discover all Evidence ({DataManager.Instance.NotebookEvidence.Count}/{evidenceData.Count})";
        progressSlider.value = (float) DataManager.Instance.NotebookEvidence.Count / (float) evidenceData.Count;
        StartCoroutine(CheckForAllEvidenceFound());
    }

    private IEnumerator CheckForAllEvidenceFound()
    {
        // Wait a frame for evidence to be added to notebook
        yield return null;

        // Check the player's notebook
        foreach (EvidenceData evidence in evidenceData)
        {
            if (!DataManager.Instance.CheckIfEvidenceAlreadyExists(evidence)) { yield break; }
        }

        print("OnAllEvidenceFound");
        OnAllEvidenceFound?.Invoke();
    }

    // DEV-ONLY: Automatically finds all evidence in the scene
    public void FindAllEvidenceDebug()
    {
        foreach (var evidence in evidenceData)
        {
            notebook?.AddEvidence(evidence);
        }

        OnAllEvidenceFound?.Invoke();

        Debug.Log("[DEBUG] All evidence added");
    }

    private void UpdateCanvas(PlayerStates state)
    {
        // progressUI.enabled = state == PlayerStates.Move;
    }
}
