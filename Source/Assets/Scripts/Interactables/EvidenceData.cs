using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EvidenceData", menuName = "ScriptableObjects/CreateEvidenceData", order = 1)]
public class EvidenceData : ScriptableObject
{

    public EvidenceKey KeyEvidence;
    public string EvidenceName;
    public string Description;
    public Sprite DisplayImage;
    //AudioClip pickupSound?
    
}

public enum EvidenceKey
{
    BOOK,
    NONE,
    CIGAR
}