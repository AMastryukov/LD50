using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectNotebookEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private SuspectData suspectData;
    
    public void Initialize(SuspectData data)
    {
        suspectData = data;
        text.text = $"<b>{data.Name}</b>\n{data.Description}";
    }
}
