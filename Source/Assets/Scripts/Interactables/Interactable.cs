using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Interactable : MonoBehaviour, IInteractable
{ 
    public UnityEvent onInteract;
    [HideInInspector] public int id;

   private void Start()
    {
        id = Random.Range(0, 999999);
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted");
    }

    public virtual string GetName()
    {
        return "";
    }
}
