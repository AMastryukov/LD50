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
    [SerializeField] private CanvasGroup homePage;
    [SerializeField] private LogsTab logsTab;
    [SerializeField] private EvidenceTab evidenceTab;
    [SerializeField] private SuspectTab suspectTab;
    [SerializeField] private CanvasGroup notebookCG;
    [SerializeField] private CanvasGroup miniNotebookCG;

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
        tabs = new List<NotebookTab> { logsTab, evidenceTab, suspectTab};
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
            if(FindObjectOfType<PlayerManager>().CurrentState != PlayerManager.PlayerStates.Interrogate)
                FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Move;
            PlayerInteractor.OnModifyDOF(false);
            UnHighlightAllTabs();

            notebookCG.alpha = 0f;
            notebookCG.interactable = false;
            notebookCG.blocksRaycasts = false;

            miniNotebookCG.alpha = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            AudioManager.Instance.PlayNotebookFlipSound();
        }
        else
        {
            if(FindObjectOfType<PlayerManager>().CurrentState == PlayerManager.PlayerStates.Wait || FindObjectOfType<PlayerManager>().CurrentState == PlayerManager.PlayerStates.Inspect)
                return;
            if(FindObjectOfType<PlayerManager>().CurrentState!=PlayerManager.PlayerStates.Interrogate)
                FindObjectOfType<PlayerManager>().CurrentState = PlayerManager.PlayerStates.Wait;
            PlayerInteractor.OnModifyDOF(true);

            notebookCG.alpha = 1f;
            notebookCG.interactable = true;
            notebookCG.blocksRaycasts = true;

            miniNotebookCG.alpha = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            homePage.alpha = 1f;

            AudioManager.Instance.PlayNotebookFlipSound();
        }

        isOpen = !isOpen;
    }

    public void HighlightTab(int n)
    {
        UnHighlightAllTabs();
        tabs[n].Highlight();
        AudioManager.Instance.PlayNotebookFlipSound();
    }

    private void UnHighlightAllTabs()
    {
        homePage.alpha = 0f;

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
        evidenceTab.ClearEvidence();
    }
}
