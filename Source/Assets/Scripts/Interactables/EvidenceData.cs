using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EvidenceData", menuName = "ScriptableObjects/CreateEvidenceData", order = 1)]
public class EvidenceData : ScriptableObject {
    
    public new string name;
    public string description;
    //Sprite for notebook 
    //AudioClip pickupSound?
}
