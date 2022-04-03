using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "SuspectData", menuName = "ScriptableObjects/SuspectData", order = 2)]
public class SuspectScriptableObject : ScriptableObject
{
    public string SuspectName;
    public string SuspectDescription;
    public Sprite SuspectDisplayImage;
    public EvidenceData[] SuspectsKeyEvidences;
}
