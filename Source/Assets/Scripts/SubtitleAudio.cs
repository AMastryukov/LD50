using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Subtitle Audio", menuName = "ScriptableObjects/Subtitle Audio", order = 1)]
public class SubtitleAudio : ScriptableObject
{
    public string prefabName;

    public AudioClip clip;

    [TextArea]
    public string subtitle;
}
