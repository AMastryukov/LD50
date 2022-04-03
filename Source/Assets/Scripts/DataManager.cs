using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityCode;

public class DataManager : UnitySingletonPersistent<DataManager>
{
    public List<string> LogsListInNotebook { get; private set; }
    public List<EvidenceKey> evidenceListInNotebook{ get; private set; }
    public List<PersonnelData> personnelListInNotebook { get; private set; }
    
    
    [SerializeField] private SuspectScriptableObject[] suspectData;
    [SerializeField] private EvidenceData[] evidenceData;
   // [SerializeField] private VoiceLineData[] voiceLines;

    //public SuspectScriptableObject GetSuspectData(string name);
    //public SuspectScriptableObject GetEvidenceData(string evidenceID);
    //public SuspectScriptableObject GetVoiceLineData(string voicelineID);

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

    public bool CheckIfEvidenceAlreadyExists(EvidenceKey key)
    {
        foreach (var evidenceData in evidenceListInNotebook)
        {
            if (evidenceData == key)
            {
                return true;
            }
        }
        return false;
    }
    
    public bool CheckIfPersonnelAlreadyExists(EvidenceKey key)
    {
        foreach (var evidenceData in personnelListInNotebook)
        {
            if (evidenceData.EvidenceKey == key)
            {
                return true;
            }
        }
        return false;
    }

    public EvidenceData GetEvidenceDataFromKey(EvidenceKey key)
    {
        foreach (var evidence in evidenceData)
        {
            if (evidence.KeyEvidence == key)
            {
                return evidence;
            }
        }

        return null;
    }
}
