using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour, IInteractable
{ 
    public UnityEvent OnInteract;

    public virtual void Interact()
    {
        Debug.Log("Interacted");
    }

    public virtual string GetName()
    {
        return "";
    }
}
