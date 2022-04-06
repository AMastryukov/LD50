using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] private List<Suspect> Suspects;

    private Suspect currentSuspect;
    public Suspect CurrentSuspect
    {
        get
        {
            return currentSuspect;
        }
    }

    private bool IsInterrogating;

    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private Canvas progressUI;
    [SerializeField] private Slider progressSlider;

    private void Awake()
    {
        if (Suspects == null || Suspects.Count < 3) 
        { 
            Debug.LogError("Missing Suspects");
        }

        OnPlayerStateChanged += CheckInterrogating;
    }

    private void CheckInterrogating(PlayerStates state)
    {
        if (IsInterrogating && state != PlayerStates.Interrogate)
        {
            EvidenceNotebookEntry.OnEvidenceSelectedInNotebook -= PresentEvidenceToSuspect;
            IsInterrogating = false;
        }
        else if (!IsInterrogating && state == PlayerStates.Interrogate)
        {
            EvidenceNotebookEntry.OnEvidenceSelectedInNotebook += PresentEvidenceToSuspect;
            IsInterrogating = true;
        }
    }

    private void OnDestroy()
    {
        OnPlayerStateChanged -= CheckInterrogating;
    }

    private void PresentEvidenceToSuspect(EvidenceData evidenceData)
    {
        StartCoroutine(Interrogate(evidenceData));
    }

    private IEnumerator Interrogate(EvidenceData evidenceData)
    {
        yield return currentSuspect.AskAboutEvidence(evidenceData);

        var progress = currentSuspect.Data.KeyEvidence.Count - currentSuspect.RemainingKeyEvidence.Count;

        objectiveText.text = $"Ask about relevant Evidence ({progress}/{currentSuspect.Data.KeyEvidence.Count})";
        progressSlider.value = (float)progress / (float)currentSuspect.Data.KeyEvidence.Count;
    }

    public void SetCurrentSuspect(string suspectName)
    {
        Suspect suspect = Suspects.FirstOrDefault(sussy => sussy.Data.Name == suspectName);

        currentSuspect = suspect;

        foreach (var sus in Suspects)
        {
            // Match the game object by name
            sus.gameObject.SetActive(sus.Data.Name.Equals(currentSuspect.Data.Name));
        }

        progressSlider.value = 0;
        objectiveText.text = $"Ask about relevant Evidence ({0}/{currentSuspect.Data.KeyEvidence.Count})";
    }

    public void DebugForceConfession()
    {
        if (currentSuspect == null) { Debug.LogError("There is no current suspect!"); return; }

        Suspect.OnConfess?.Invoke(currentSuspect);

        Debug.Log($"[DEBUG] Forced confession from {currentSuspect.Data.Name}");
    }
}
