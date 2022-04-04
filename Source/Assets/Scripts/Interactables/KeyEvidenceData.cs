using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyEvidenceData", menuName = "ScriptableObjects/KeyEvidenceData", order = 3)]
public class KeyEvidenceData : ScriptableObject
{
    public EvidenceData Evidence;
    public AudioClip VoiceLine;
}
