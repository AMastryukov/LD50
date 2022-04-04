using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EvidenceNotebookEntry : MonoBehaviour
{
    public static Action<EvidenceData> OnEvidenceSelectedInNotebook;

    [SerializeField] private TextMeshProUGUI text;

    private EvidenceData evidenceData;
    
    public void InitializeEvidence(EvidenceData data)
    {
        evidenceData = data;
        text.text = $"<b>{data.Name}</b>\n{data.Description}";
    }

    public void EvidenceClicked()
    {
        OnEvidenceSelectedInNotebook?.Invoke(evidenceData);
    }
}
