using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", menuName = "Scriptable Objects/Evidence", order = 1)]
public class ScriptableEvidence : ScriptableObject
{
    public ItemKey ItemKey;
    public string DisplayName;
    public string Description;
    public Sprite DisplayImage;
}

public enum ItemKey { 
    Bottle,
    Lighter,
    Dagger,
}

