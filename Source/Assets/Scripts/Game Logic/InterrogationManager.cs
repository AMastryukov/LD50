using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManager;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> suspectObjects;

    private Suspect currentSuspect;

    private bool IsInterrogating;

    private void Awake()
    {
        if (suspectObjects == null || suspectObjects.Capacity == 0) 
        { 
            Debug.LogError("There is no suspect objects in the list!");
            return; 
        }

        OnPlayerStateChanged += CheckInterrogating;
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
            print("Now Interrogating");
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

        StartCoroutine(Interrogate(evidenceData));
    }

    private IEnumerator Interrogate(EvidenceData evidenceData)
    {
        yield return currentSuspect.AskAboutEvidence(evidenceData);
        
    }

    public void SetCurrentSuspect(SuspectData suspectData)
    {
        currentSuspect = new Suspect(suspectData);

        foreach (var suspectObject in suspectObjects)
        {
            // Match the game object by name
            suspectObject.SetActive(suspectObject.name.Equals(currentSuspect.Data.Name));
        }
    }

    public void DebugForceConfession()
    {
        if (currentSuspect == null) { Debug.LogError("There is no current suspect!"); return; }

        Suspect.OnConfess?.Invoke(currentSuspect);

        Debug.Log($"[DEBUG] Forced confession from {currentSuspect.Data.Name}");
    }
}
