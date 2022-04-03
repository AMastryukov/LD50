using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EvidenceObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI evidenceName;
    [SerializeField] private TextMeshProUGUI evidenceDescription;
    [SerializeField] private Image evidenceImage;
    
    public void InitializeEvidence(EvidenceData data)
    {
        evidenceName.text = data.name;
        evidenceDescription.text = data.Description;
        evidenceImage.sprite = data.Sprite;
    }
}
