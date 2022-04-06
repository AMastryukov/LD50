using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEvidenceInteractable : Interactable
{
    private CrimeSceneManager crimeSceneManager;

    private void Awake()
    {
        crimeSceneManager = FindObjectOfType<CrimeSceneManager>();

        if (crimeSceneManager == null)
        {
            Debug.LogError("Scene is missing a crime scene manager!");
            Destroy(gameObject);
        }

        if (!Application.isEditor)
        {
            Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        crimeSceneManager.FindAllEvidenceDebug();
    }
}
