using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EvidenceNotebookEntry : MonoBehaviour
{
    public static Action<EvidenceData> OnEvidenceSelectedInNotebook;

    [SerializeField] private TextMeshProUGUI evidenceName;
    [SerializeField] private TextMeshProUGUI evidenceDescription;
    [SerializeField] private Image evidenceImage;

    private EvidenceData evidenceData;
    
    public void InitializeEvidence(EvidenceData data)
    {
        evidenceData = data;

        evidenceName.text = data.name;
        evidenceDescription.text = data.Description;
        evidenceImage.sprite = data.Sprite;
    }

    public void EvidenceClicked()
    {
        OnEvidenceSelectedInNotebook?.Invoke(evidenceData);
    }
}
