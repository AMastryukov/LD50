using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoicelineSubtitles : MonoBehaviour
{
    private TextMeshProUGUI subtitleText;
    private Notebook notebook;

    private void Awake()
    {
        notebook = FindObjectOfType<Notebook>();

        subtitleText = GetComponent<TextMeshProUGUI>();
        subtitleText.text = "";
    }

    public IEnumerator ShowSubtitle(VoiceLineData voicelineData)
    {
        subtitleText.text = $"<b>{voicelineData.CharacterName}</b>: {voicelineData.Subtitle}";

        notebook?.AddLog(subtitleText.text);

        yield return new WaitForSeconds(voicelineData.AudioClip.length);

        subtitleText.text = "";
    }
}
