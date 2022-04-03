using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonnelData", menuName = "ScriptableObjects/CreatePersonnelData", order = 2)]
public class PersonnelData : ScriptableObject
{
    public EvidenceKey EvidenceKey;
    public string PersonnelName;
    public string Description;
    public Sprite DisplayImage;
}
