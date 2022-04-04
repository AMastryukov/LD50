using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static PlayerManager;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] private List<Suspect> Suspects;

    private Suspect currentSuspect;

    private bool IsInterrogating;

    [SerializeField] private Canvas progressUI;
    [SerializeField] private Slider progressSlider;

    private void Awake()
    {
        if (Suspects == null || Suspects.Count < 3) 
        { 
            Debug.LogError("Missing Suspects");
        }

        OnPlayerStateChanged += CheckInterrogating;

        progressSlider.value = 0;
    }

    private void CheckInterrogating(PlayerStates state)
    {
        if (IsInterrogating && state != PlayerStates.Interrogate)
        {
            print("Stop interrogating");
            EvidenceNotebookEntry.OnEvidenceSelectedInNotebook -= PresentEvidenceToSuspect;
            IsInterrogating = false;
        }
        else if (!IsInterrogating && state == PlayerStates.Interrogate)
        {
            print("Now Interrogating " + currentSuspect.Data.Name);
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
        print("Evidence Being presented to suspect");
        progressSlider.value = (float) currentSuspect.RemainingKeyEvidence.Count / (float) currentSuspect.Data.KeyEvidence.Count;
        StartCoroutine(Interrogate(evidenceData));
    }

    private IEnumerator Interrogate(EvidenceData evidenceData)
    {
        yield return currentSuspect.AskAboutEvidence(evidenceData);
        
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
    }

    public void DebugForceConfession()
    {
        if (currentSuspect == null) { Debug.LogError("There is no current suspect!"); return; }

        Suspect.OnConfess?.Invoke(currentSuspect);

        Debug.Log($"[DEBUG] Forced confession from {currentSuspect.Data.Name}");
    }
}
