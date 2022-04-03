using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EvidenceData", menuName = "ScriptableObjects/CreateEvidenceData", order = 1)]
public class EvidenceData : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
}