using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Suspect : MonoBehaviour
{
    public static Action<Suspect, EvidenceData> OnKeyEvidenceShown;
    public static Action<Suspect> OnGenericEvidenceShown;
    public static Action<Suspect> OnConfess;

    public SuspectData Data;

    /// <summary>
    /// Every time the player brings up the matching key evidence, we remove a string from the list.
    /// When the list is empty, the suspect confesses
    /// </summary>
    private List<string> RemainingKeyEvidence;


    public void Start()
    {
        RemainingKeyEvidence = new List<string>();

        foreach (KeyEvidenceData keyev in Data.KeyEvidence)
        {
            RemainingKeyEvidence.Add(keyev.Evidence.Name);
        }
    }

    public IEnumerator AskAboutEvidence(EvidenceData evidenceData)
    {
        yield return null;

        KeyEvidenceData keyev = Data.KeyEvidence.FirstOrDefault(keyev => keyev.Evidence.Name == evidenceData.Name);

        if (keyev)
        {
            // Remove from the remaining key evidence list
            RemainingKeyEvidence.Remove(evidenceData.Name);
            OnKeyEvidenceShown?.Invoke(this, evidenceData);

            Debug.Log("Key Evidence Found");
            yield return sayLine(keyev.VoiceLine);

            if (RemainingKeyEvidence.Count == 0)
            {
                Debug.Log("Confessing");
                OnConfess?.Invoke(this);
                yield return sayLine(Data.Confession);
            }
        }
        else
        {
            Debug.Log("Generic Evidence Found");
            OnGenericEvidenceShown?.Invoke(this);
        }
    }

    public IEnumerator sayLine(AudioClip clip)
    {
        Debug.LogError("Missing VoiceLine");
        yield return null;
    }
}
