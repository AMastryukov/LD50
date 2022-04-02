using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Tabs { 
    LOG,
    EVIDENCE,
    PERSONNEL
}

public class Notebook : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isOpen = false;
    [SerializeField] private List<NotebookTab> tabs;

    [SerializeField]
    private EvidenceTab EvidenceTab;

    [SerializeField]
    private PersonnelTab PersonnelTab;

    [SerializeField]
    private LogsTab LogsTab;

    // Start is called before the first frame update
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCanvas();
        }
    }
    private void ToggleCanvas()
    {
        if (isOpen)
        {
            UnHighlightAllTabs();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        isOpen = !isOpen;
    }

    public void HighlightTab(int n)
    {
        UnHighlightAllTabs();
        tabs[n].Highlight();
        Debug.Log("ButtonPressed");
    }

    private void UnHighlightAllTabs()
    {
        foreach (var tab in tabs)
        {
            tab.UnHighlight();
        }
    }

    public void AddEvidence(EvidenceData evidence)
    {
        EvidenceTab.Add(evidence);
    }

    public void AddPersonnel(PersonnelData personnel)
    {
        PersonnelTab.Add(personnel);
    }

    public void AddLog(string log)
    {
        LogsTab.Add(log);
    }
}