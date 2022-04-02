using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    
    public void InitializeLog(string text)
    {
        logText.text = text;
    }
}
