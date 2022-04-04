using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SuspectData", menuName = "ScriptableObjects/SuspectData", order = 2)]
public class SuspectData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public List<KeyEvidenceData> KeyEvidence;
    public List<AudioClip> GenericResponses;
    public AudioClip Confession;
}
