using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] PlayerManager PM;
    //[SerializeField] EvidenceNotebookEntry EO;
    [SerializeField] Suspect Sus;
    [SerializeField] InterrogationBench Bench;


    // Start is called before the first frame update
    void Start()
    {
        if (PM == null) Debug.LogError("PlayerManager is missing");
        if (Sus == null) Debug.LogError("Suspect is missing");
        if (Bench == null) Debug.LogError("InterrogationBench is missing");

        
        // Bench.SitDown += StartInterrogation;
        // Bench.GetUp += StopInterrogation;

    }

    public void StartInterrogation() {
        Debug.Log("You are now interrogating the suspect");
        // PM.PresentEvidence += PresentEvidence;
    }

    public void StopInterrogation()
    {
        Debug.Log("You have stopped interrogating the suspect");
        // Unsub from the present evidence
        // PM.PresentEvidence -= PresentEvidence;
    }

    public void PresentEvidence() {
        // Disable Player
    }

    public void SuspectDoneSpeaking() {
        // Enable Player
    }
}
