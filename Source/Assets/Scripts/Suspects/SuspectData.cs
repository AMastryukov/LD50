using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "SuspectData", menuName = "ScriptableObjects/SuspectData", order = 2)]
public class SuspectData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public List<EvidenceData> KeyEvidence;
}

[Serializable]
public class Suspect
{
    public static Action<Suspect, EvidenceData> OnKeyEvidenceShown;
    public static Action<Suspect> OnGenericEvidenceShown;
    public static Action<Suspect> OnConfess;

    public SuspectData Data;

    /// <summary>
    /// Every time the player brings up the matching key evidence, we remove a string from the list.
    /// When the list is empty, the suspect confesses
    /// </summary>
    public List<string> RemainingKeyEvidence;

    public Suspect(SuspectData suspectData)
    {
        Data = suspectData;
        RemainingKeyEvidence = new List<string>();

        foreach (var evidence in suspectData.KeyEvidence)
        {
            RemainingKeyEvidence.Add(evidence.Name);
        }
    }

    public IEnumerator AskAboutEvidence(EvidenceData evidenceData)
    {
        yield return null;
        if (Data.KeyEvidence.Contains(evidenceData))
        {
            // Remove from the remaining key evidence list
            RemainingKeyEvidence.Remove(evidenceData.Name);
            OnKeyEvidenceShown?.Invoke(this, evidenceData);

            // TODO yield return evidenceData.voiceLine;

            if (RemainingKeyEvidence.Count == 0)
            {
                OnConfess?.Invoke(this);
                // TODO yield return suspect.voiceLine;
            }
        }
        else
        {
            OnGenericEvidenceShown?.Invoke(this);
        }
    }
}
