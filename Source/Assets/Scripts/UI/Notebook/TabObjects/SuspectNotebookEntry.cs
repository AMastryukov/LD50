using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectNotebookEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    private SuspectData suspectData;
    
    public void Initialize(SuspectData data)
    {
        suspectData = data;
        text.text = $"<b>{data.Name}</b>\n{data.Description}";
        image.sprite = data.Sprite;
    }
}
