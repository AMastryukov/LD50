using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VoiceLineData", menuName = "ScriptableObjects/VoiceLineData", order = 4)]
public class VoiceLineData : ScriptableObject
{
    public AudioClip AudioClip;
    public string CharacterName;
    [TextArea] public string Subtitle;
}
