using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectNotebookEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI suspectName;
    [SerializeField] private TextMeshProUGUI suspectDescription;
    [SerializeField] private Image suspectImage;

    private SuspectData suspectData;
    
    public void Initialize(SuspectData data)
    {
        suspectData = data;
        suspectName.text = data.name;
        suspectDescription.text = data.Description;
        suspectImage.sprite = data.Sprite;
    }
}
