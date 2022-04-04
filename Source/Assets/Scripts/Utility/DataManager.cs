using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class DataManager : UnitySingletonPersistent<DataManager>
{
    public List<string> NotebookLog { get; private set; } = new List<string>();
    public List<EvidenceData> NotebookEvidence{ get; private set; } = new List<EvidenceData>();

    internal VoiceLineData GetRandomGenericPlayerEvidenceVoiceline()
    {
        // I think you should put this somewhere else but up to you
        throw new NotImplementedException();
    }

    public List<SuspectData> NotebookSuspects { get; private set; } = new List<SuspectData>();

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
}
