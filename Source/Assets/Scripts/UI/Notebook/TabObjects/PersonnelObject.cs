using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonnelObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI personnelName;
    [SerializeField] private TextMeshProUGUI personnelDescription;
    [SerializeField] private Image personnelImage;
    
    
    public void InitializeEvidence(PersonnelData data)
    {
        personnelName.text = data.name;
        personnelDescription.text = data.Description;
        personnelImage.sprite = data.DisplayImage;
    }
}
