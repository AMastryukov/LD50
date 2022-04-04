using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class DataManager : UnitySingletonPersistent<DataManager>
{
    public List<string> NotebookLog { get; private set; } = new List<string>();
    public List<string> NotebookEvidence{ get; private set; } = new List<string>();
    public List<string> NotebookSuspects { get; private set; } = new List<string>();


    [SerializeField] private SuspectData[] suspectData;
    [SerializeField] private EvidenceData[] evidenceData;
    [SerializeField] private VoiceLineData[] voiceLineData;
    [SerializeField] private VoiceLineData[] genericPlayerEvidencePromptsMale;
    [SerializeField] private VoiceLineData[] genericPlayerEvidencePromptsFemale;

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

    public bool CheckIfEvidenceAlreadyExists(string name)
    {
        foreach (var evidenceName in NotebookEvidence)
        {
            if (evidenceName == name)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckIfSuspectAlreadyExists(string name)
    {
        foreach (var suspectName in NotebookSuspects)
        {
            if (suspectName == name)
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
        foreach (var voiceLine in voiceLineData)
        {
            if (voiceLine.name == name)
            {
                return voiceLine;
            }
        }

        return null;
    }

    public VoiceLineData GetRandomGenericPlayerEvidenceVoiceline()
    {
        VoiceLineData[] voiceLines;

        if (PlayerPrefs.GetString("PlayerSex", "MALE").Equals("MALE"))
        {
            voiceLines = genericPlayerEvidencePromptsMale;
        }
        else
        {
            voiceLines = genericPlayerEvidencePromptsFemale;
        }

        return voiceLines[UnityEngine.Random.Range(0, voiceLines.Length)];
    }
}
