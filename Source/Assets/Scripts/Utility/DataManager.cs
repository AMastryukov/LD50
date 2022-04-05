using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class DataManager : UnitySingletonPersistent<DataManager>
{
    public List<string> NotebookLog { get; private set; } = new List<string>();
    public List<EvidenceData> NotebookEvidence{ get; private set; } = new List<EvidenceData>();
    public List<SuspectData> NotebookSuspects { get; private set; } = new List<SuspectData>();

    [SerializeField] private SuspectData[] suspectData;
    [SerializeField] private EvidenceData[] evidenceData;
    [SerializeField] private VoiceLineData[] voiceLineData;
    [SerializeField] private VoiceLineData[] genericPlayerEvidencePromptsMale;
    [SerializeField] private VoiceLineData[] genericPlayerEvidencePromptsFemale;
    [SerializeField] private AudioClip[] soundEffects;
    [SerializeField] private AudioClip[] voicemail;

    public bool CheckIfLogAlreadyExists(string log)
    {
        foreach (var logText in NotebookLog)
        {
            if (logText == log)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool CheckIfEvidenceAlreadyExists(EvidenceData evidence)
    {
        foreach (var ev in NotebookEvidence)
        {
            if (evidence.Name == ev.Name)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckIfSuspectAlreadyExists(SuspectData suspect)
    {
        foreach (var sus in NotebookSuspects)
        {
            if (suspect.Name == sus.Name)
            {
                return true;
            }
        }
        return false;
    }


    public EvidenceData GetEvidenceDataFromKey(string name)
    {
        foreach (var evidence in evidenceData)
        {
            if (evidence.name == name)
            {
                return evidence;
            }
        }

        return null;
    }

    public SuspectData GetSuspectDataFromKey(string name)
    {
        foreach (var suspect in suspectData)
        {
            if (suspect.name == name)
            {
                return suspect;
            }
        }

        return null;
    }

    public VoiceLineData GetVoiceLineDataFromKey(string key)
    {
        // Add MALE or FEMALE to the player voiceline key at the end
        if (key.StartsWith("PLAYER_"))
        {
            key += PlayerPrefs.GetString("PlayerVoice", "MALE").Equals("MALE") ? "_MALE" : "_FEMALE";
        }

        foreach (var voiceLine in voiceLineData)
        {
            if (voiceLine.name == key)
            {
                return voiceLine;
            }
        }

        Debug.LogError($"[DataManager] Could not find voiceline with key {key}");

        return null;
    }

    public VoiceLineData GetRandomGenericPlayerEvidenceVoiceline()
    {
        VoiceLineData[] voiceLines;

        if (PlayerPrefs.GetString("PlayerVoice", "MALE").Equals("MALE"))
        {
            voiceLines = genericPlayerEvidencePromptsMale;
        }
        else
        {
            voiceLines = genericPlayerEvidencePromptsFemale;
        }

        return voiceLines[UnityEngine.Random.Range(0, voiceLines.Length)];
    }

    public AudioClip GetSoundEffect(string key)
    {
        foreach (var effect in soundEffects)
        {
            if (effect.name == key)
            {
                return effect;
            }
        }

        Debug.LogError($"[DataManager] Could not find sound effect with key {key}");

        return null;
    }

    public AudioClip GetVoiceMail()
    {
        foreach (var mail in voicemail)
        {
            return mail;
        }

        Debug.LogError($"[DataManager] Could not find voicemail");

        return null;
    }
}
