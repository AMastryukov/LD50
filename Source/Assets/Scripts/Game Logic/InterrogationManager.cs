using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterrogationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> suspectObjects;

    private Suspect currentSuspect;

    private void Awake()
    {
        if (suspectObjects == null || suspectObjects.Capacity == 0) 
        { 
            Debug.LogError("There is no suspect objects in the list!");
            return; 
        }
    }

    public void SetCurrentSuspect(SuspectData suspectData)
    {
        currentSuspect = new Suspect(suspectData);

        foreach (var suspectObject in suspectObjects)
        {
            // Match the game object by name
            suspectObject.SetActive(suspectObject.name.Equals(currentSuspect.Data.Name));
        }
    }

    public void DebugForceConfession()
    {
        if (currentSuspect == null) { Debug.LogError("There is no current suspect!"); return; }

        Suspect.OnConfess?.Invoke(currentSuspect);

        Debug.Log($"[DEBUG] Forced confession from {currentSuspect.Data.Name}");
    }
}
