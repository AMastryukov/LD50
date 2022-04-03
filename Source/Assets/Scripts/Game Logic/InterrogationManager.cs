using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManager;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] PlayerManager PM;
    [SerializeField] EvidenceNotebookEntry EO;
    [SerializeField] Suspect Sus;
    [SerializeField] InterrogationBench Bench;


    // Start is called before the first frame update
    void Start()
    {
        if (PM == null) Debug.LogError("PlayerManager is missing");
        if (Sus == null) Debug.LogError("Suspect is missing");
        if (Bench == null) Debug.LogError("InterrogationBench is missing");

        PlayerManager.PlayerStateChanged += PlayerStateChanged;

    }

    public void PlayerStateChanged(PlayerStates state)
    {
        if (state == PlayerStates.Interrogate)
        {
            print("Interrogating");
            // PM.PresentEvidence += PresentEvidence;
        }
        else
        {
            print("Stopped Interrogating");
            // PM.PresentEvidence -= PresentEvidence;
        }
    }

    public void PresentEvidence() {
        // Disable Player
    }

    public void SuspectDoneSpeaking() {
        // Enable Player
    }
}
