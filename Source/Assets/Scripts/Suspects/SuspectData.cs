using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SuspectData", menuName = "ScriptableObjects/SuspectData", order = 2)]
public class SuspectData : ScriptableObject
{
    public string Name;
    [TextArea] public string Description;
    public Sprite Sprite;
    public List<EvidenceData> KeyEvidence;

    [Header("Voicelines")]
    public List<VoiceLineData> KeyEvidenceVoicelines;
    public List<VoiceLineData> GenericEvidenceVoicelines;
    public VoiceLineData IntroductionVoiceline;
    public VoiceLineData ConfessionVoiceline;
}