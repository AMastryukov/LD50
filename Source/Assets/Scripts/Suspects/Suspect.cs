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
    [HideInInspector] public List<string> RemainingKeyEvidence { get; private set; }


    public void Start()
    {
        RemainingKeyEvidence = new List<string>();

        foreach (EvidenceData ev in Data.KeyEvidence)
        {
            RemainingKeyEvidence.Add(ev.Name);
        }
    }

    public IEnumerator AskAboutEvidence(EvidenceData evidenceData)
    {
        yield return null;

        EvidenceData evidence = Data.KeyEvidence.FirstOrDefault(ev => ev.Name == evidenceData.Name);

        if (evidence)
        {
            // Remove from the remaining key evidence list
            RemainingKeyEvidence.Remove(evidenceData.Name);
            OnKeyEvidenceShown?.Invoke(this, evidenceData);

            Debug.Log("Key Evidence Found");
            int i = Data.KeyEvidence.IndexOf(evidence);
            VoiceLineData vld = Data.KeyEvidenceVoicelines[i];

            yield return sayLine(vld.AudioClip);

            if (RemainingKeyEvidence.Count == 0)
            {
                Debug.Log("Confessing");
                OnConfess?.Invoke(this);
                yield return sayLine(Data.ConfessionVoiceline.AudioClip);
            }
        }
        else
        {
            Debug.Log("Generic Evidence Found");
            OnGenericEvidenceShown?.Invoke(this);
            yield return sayLine(Data.GenericEvidenceVoicelines[UnityEngine.Random.Range(0, Data.GenericEvidenceVoicelines.Count)].AudioClip);
        }
    }

    public IEnumerator sayLine(AudioClip clip)
    {
        Debug.LogError("Missing VoiceLine");
        // yield return new WaitForSeconds(clip.length);
        yield return null;
    }
}
