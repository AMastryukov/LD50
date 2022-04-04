using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInterrogationInteractable : Interactable
{
    private InterrogationManager interrogationManager;
    private Suspect suspect;

    private void Awake()
    {
        interrogationManager = FindObjectOfType<InterrogationManager>();

        if (interrogationManager == null)
        {
            Debug.LogError("Scene is missing an interrogation manager!");
            Destroy(gameObject);
        }
    }

    public override void Interact()
    {
        interrogationManager.DebugForceConfession();
    }
}
