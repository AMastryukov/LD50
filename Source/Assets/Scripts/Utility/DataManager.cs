using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class DataManager : UnitySingletonPersistent<DataManager>
{
    public List<string> LogsListInNotebook { get; private set; }
    public List<string> evidenceListInNotebook{ get; private set; }
    public List<string> suspectListInNotebook { get; private set; }
    
    
    [SerializeField] private SuspectData[] suspectData;
    [SerializeField] private EvidenceData[] evidenceData;

    public bool CheckIfLogAlreadyExists(string log)
    {
        foreach (var logText in LogsListInNotebook)
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
        foreach (var evidenceName in evidenceListInNotebook)
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
        foreach (var suspectName in suspectListInNotebook)
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
}
