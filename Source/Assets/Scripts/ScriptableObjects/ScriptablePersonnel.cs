using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personnel", menuName = "Scriptable Objects/Personnel", order = 2)]
public class ScriptablePersonnel : ScriptableObject
{
    public ItemKey KeyEvidence;
    public string DisplayName;
    public string Description;
    public Sprite DisplayImage;
}
