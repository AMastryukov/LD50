using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum Tabs 
{ 
    LOG,
    EVIDENCE,
    SUSPECTS
}

public class Notebook : MonoBehaviour
{
    [SerializeField] private LogsTab logsTab;
    [SerializeField] private EvidenceTab evidenceTab;
    [SerializeField] private SuspectTab suspectTab;

    private CanvasGroup canvasGroup;
    private bool isOpen = false;
    private List<NotebookTab> tabs;
    private DataManager dataManager;

    #region EventAssignement

    private void AssignDelegates()
    {
        Evidence.OnInspect += OnEvidenceInspected;
    }

    private void UnAssignDelegates()
    {
        Evidence.OnInspect -= OnEvidenceInspected;
    }

    #endregion

    #region UnityEventFunctions

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        tabs = new List<NotebookTab> {logsTab, evidenceTab, suspectTab};
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        AssignDelegates();
        Initialize();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCanvas();
        }
    }
    
    private void OnDestroy()
    {
        UnAssignDelegates();
    }
    

    #endregion

    private void Initialize()
    {
        foreach (var log in dataManager.NotebookLog)
        {
            logsTab.InstantiateLog(log);
        }

        foreach (EvidenceData evidence in dataManager.NotebookEvidence)
        {
            evidenceTab.InstantiateEvidence(evidence);
        }

        foreach (SuspectData suspect in dataManager.NotebookSuspects)
        {
            suspectTab.InstantiateSuspect(suspect);
        }
    }

    // Update is called once per frame
   
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
        AudioManager.Instance.OnNoteBookPageFlip();
    }

    private void UnHighlightAllTabs()
    {
        foreach (var tab in tabs)
        {
            tab.UnHighlight();
        }
    }

    private void OnEvidenceInspected(Evidence evidence)
    {
        AddEvidence(evidence.evidenceData);
    }

    public void AddEvidence(EvidenceData evidence)
    {
        evidenceTab.Add(evidence);
    }

    public void AddSuspect(SuspectData suspect)
    {
        suspectTab.Add(suspect);
    }

    public void AddLog(string log)
    {
        logsTab.Add(log);
    }

    public void ClearEvidence()
    {
        
    }
}
