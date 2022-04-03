using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour, IInteractable
{ 
    public UnityEvent OnInteract;

    public virtual void Interact()
    {

    }

    public virtual string GetName()
    {
        return "";
    }
}
